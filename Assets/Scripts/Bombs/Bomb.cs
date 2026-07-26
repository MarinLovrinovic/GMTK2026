using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IHittable
{
    public bool Moved { get; private set; } = false;
    public GameObject initialImage;
    public GameObject carryImage;
    public GameObject releaseImage;

    [SerializeField] protected LayerMask hittable;
    [SerializeField] protected float explosionRadius = 1;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected GameObject explosionFX;
    [SerializeField] protected TMP_Text countdownText;
    [SerializeField] protected int minTime;
    [SerializeField] protected int maxTime;
    [SerializeField] protected bool alwaysShowRadius;
    [SerializeField] protected LineRenderer radiusDisplay;
    public Vector2 Position => transform.position.xz();

    public void SetRadiusDisplay(bool value)
    {
        if (!radiusDisplay) return;
        if (!alwaysShowRadius)
        {
            radiusDisplay.enabled = value;
        }
    }

    protected void UpdateDisplayRadius(float radius)
    {
        if (!radiusDisplay) return;
        int segmentCount = 36;
        float angleIncrement = 360f / segmentCount;
        Vector3[] points = new Vector3[segmentCount + 1];
        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = i * angleIncrement;
            points[i] = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0.2f, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
        }
        radiusDisplay.positionCount = points.Length;
        radiusDisplay.SetPositions(points);
    }

    protected int timeUntilExplosion = 5;
    protected bool explosionStarted = false;
    protected bool isFrozen;

    private void Start()
    {
        if (radiusDisplay)
        {
            Debug.Log("radius display, always show: " + alwaysShowRadius.ToString());
            radiusDisplay.enabled = alwaysShowRadius;
            UpdateDisplayRadius(explosionRadius);
        }
        timeUntilExplosion = Random.Range(minTime, maxTime);
        countdownText.text = (timeUntilExplosion).ToString();
        imageControl(true, false, false);
        isFrozen = false;
    }

    private void OnEnable()
    {
        TickDriver.instance.OnTick += Tick;
    }

    private void OnDisable()
    {
        TickDriver.instance.OnTick -= Tick;
    }

    void Tick()
    {
        if (isFrozen) return;

        timeUntilExplosion -= 1;
        countdownText.text = (timeUntilExplosion).ToString();
        perTickLogic();

        if (timeUntilExplosion <= 0)
        {
            preExplosionLogic(true);
            Explode(true);
        }
    }

    private void imageControl(bool initial, bool carry, bool release)
    {
        initialImage.SetActive(initial);
        carryImage.SetActive(carry);
        releaseImage.SetActive(release);
    }

    private void Explode(bool isSelfCaused)
    {
        // ensure only one explode call per bomb
        if (explosionStarted) return;
        explosionStarted = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, hittable);
        foreach (Collider hit in hits)
        {
            IHittable hittable = hit.GetComponent<IHittable>();
            if (hittable != null && !ReferenceEquals(hittable, this)) explosionLogic(hittable, isSelfCaused);
        }
        GameObject effect = Instantiate(explosionFX, transform.position, Quaternion.identity);
        effect.GetComponent<Explosion>().sizeAndDestroy(explosionRadius);
        //float scale = 2.2f * explosionRadius;
        //effect.transform.localScale = new Vector3(scale, scale, scale);

        Destroy(gameObject);
    }

    private IEnumerator waitFrozen(float time)
    {
        isFrozen = true;
        yield return new WaitForSeconds(time);
        isFrozen = false;
    }

    protected virtual void perTickLogic() { }

    protected virtual void preExplosionLogic(bool isSelfCaused) { }

    protected virtual void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        hittable.Hit(damage);
    }

    public void Move()
    {
        Moved = true;
        imageControl(false, true, false);
    }

    public void Release()
    {
        imageControl(false, false, true);
    }

    public void Hit(float damage)
    {
        preExplosionLogic(false);
        Explode(false);
    }

    public void Freeze(float time)
    {
        StartCoroutine(waitFrozen(time));
    }

    public float getExplosionRadius()
    {
        return explosionRadius;
    }
}
