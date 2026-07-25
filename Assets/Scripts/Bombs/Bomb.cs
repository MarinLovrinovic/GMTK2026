using TMPro;
using System.Collections;
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

    protected int timeUntilExplosion = 5;
    protected bool explosionStarted = false;
    protected bool isFrozen;

    private void Start()
    {
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
            preExplosionLogic();
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
            if (!ReferenceEquals(hittable, this)) explosionLogic(hittable, isSelfCaused);
        }
        GameObject effect = Instantiate(explosionFX, transform.position, Quaternion.identity);
        float scale = 2.2f * explosionRadius;
        effect.transform.localScale = new Vector3(scale, scale, scale);
        
        Destroy(gameObject);
    }

    private IEnumerator waitFrozen(float time)
    {
        isFrozen = true;
        yield return new WaitForSeconds(time);
        isFrozen = false;
    }

    protected virtual void perTickLogic() { }

    protected virtual void preExplosionLogic() { }

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
