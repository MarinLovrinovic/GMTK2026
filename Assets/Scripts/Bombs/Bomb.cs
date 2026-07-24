using TMPro;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IHittable
{
    public bool Moved { get; private set; } = false;
    public GameObject initialImage;
    public GameObject carryImage;
    public GameObject releaseImage;

    [SerializeField] private LayerMask hittable;
    [SerializeField] private float explosionRadius = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private GameObject explosionFX;
    [SerializeField] private TMP_Text countdownText;

    private float timeUntilExplosion = 5;
    private bool explosionStarted = false;
    private bool isFrozen;

    private void Start()
    {
        timeUntilExplosion = Random.Range(4, 13);
        imageControl(true, false, false);
        isFrozen = false;
    }

    void Update()
    {
        if (!isFrozen)
        {
            timeUntilExplosion -= Time.deltaTime;
            countdownText.text = ((int)timeUntilExplosion + 1).ToString();
            if (timeUntilExplosion <= 0)
            {
                Explode();
            }
        }
    }

    private void imageControl(bool initial, bool carry, bool release)
    {
        initialImage.SetActive(initial);
        carryImage.SetActive(carry);
        releaseImage.SetActive(release);
    }

    private void Explode()
    {
        // ensure only one explode call per bomb
        if (explosionStarted) return;
        explosionStarted = true;
        
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, hittable);
        foreach (Collider hit in hits)
        {
            IHittable hittable = hit.GetComponent<IHittable>();
            if (!ReferenceEquals(hittable, this)) explosionLogic(hittable);
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

    protected virtual void explosionLogic(IHittable hittable)
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
        Explode();
    }

    public void Freeze(float time)
    {
        StartCoroutine(waitFrozen(time));
    }
}
