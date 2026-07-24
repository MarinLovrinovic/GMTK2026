using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour, IHittable
{
    public bool Moved { get; private set; } = false;
    public GameObject initialImage;
    public GameObject carryImage;
    public GameObject releaseImage;

    [SerializeField] private LayerMask hittable;
    [SerializeField] private float explosionRadius = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private Color movedColor;

    private float timeUntilExplosion = 5;
    private bool explosionStarted = false;

    private void Start()
    {
        timeUntilExplosion = Random.Range(4, 13);
        imageControl(true, false, false);
    }

    void Update()
    {
        timeUntilExplosion -= Time.deltaTime;
        countdownText.text = ((int) timeUntilExplosion + 1).ToString();
        if (timeUntilExplosion <= 0)
        {
            Explode();
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
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position.xy(), explosionRadius, hittable);
        foreach (Collider2D hit in hits)
        {
            IHittable hittable = hit.GetComponent<IHittable>();
            if (ReferenceEquals(hittable, this))
                continue;
            hittable.Hit(damage);
        }
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        float scale = 2.2f * explosionRadius;
        effect.transform.localScale = new Vector3(scale, scale, scale);
        
        Destroy(gameObject);
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
}
