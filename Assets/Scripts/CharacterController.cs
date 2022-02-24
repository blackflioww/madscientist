using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Camera mainCamera;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    public GameObject spawnPoint;
    public GameObject ustAna, portre;
    public GameObject yazi;
    bool mouseClicked = false;
    Animator _animator;

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.D)){
            _animator.SetBool("yuruyorMu", false);
        }
        // Movement controls
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
            transform.transform.Translate(new Vector2(moveDirection,0)* maxSpeed* Time.deltaTime);
            _animator.SetBool("yuruyorMu", true);
        }
        else
        {
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
            }
        }

        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mouseClicked = true;
        }

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }

        
        
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Deadzone")){
            transform.position = spawnPoint.transform.position;
        } else if(other.gameObject.CompareTag("newLevel")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

        if(other.gameObject.CompareTag("hikayeAcici1")){
            mouseClicked = false;
            GameObject.FindGameObjectWithTag("hikayeAcici1").gameObject.SetActive(false);
            StartCoroutine(scene1Hikaye());   
        }
        if(other.gameObject.CompareTag("hikayeAcici2")){
            mouseClicked = false;
            GameObject.FindGameObjectWithTag("hikayeAcici2").gameObject.SetActive(false);
            StartCoroutine(scene2Hikaye());   
        }
        if(other.gameObject.CompareTag("hikayeAcici3")){
            mouseClicked = false;
            GameObject.FindGameObjectWithTag("hikayeAcici3").gameObject.SetActive(false);
            StartCoroutine(scene3Hikaye());   
        }
    }

    IEnumerator scene1Hikaye(){
        ustAna.SetActive(true);
        yazi.GetComponent<TextMeshProUGUI>().SetText("Dondurma, kafamdan atmaya çalıştığım kötü anıların temeli. O gün izinliydim ve kızımla parka gideceğime söz vermiştim.");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        yazi.GetComponent<TextMeshProUGUI>().SetText("Fakat kıyametimin başlangıcı da bu söz oldu... Sırada gereğinden fazla kalmıştık arkamızda iri yarı bir adam vardı ve...");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        yazi.GetComponent<TextMeshProUGUI>().SetText("Ve sürekli öfkeli bir şekilde homurdanarak, “Nerede bunlar?” diyordu. (Ağlar) Bu adamın hayatımı alt üst edeceğini nereden bilebilirdim?");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        ustAna.SetActive(false);
    }
    IEnumerator scene3Hikaye(){
        ustAna.SetActive(true);
        yazi.GetComponent<TextMeshProUGUI>().SetText("Canım kızım, eğer yaşasaydı şimdi burada onunla oynuyor olabilirdik... En sevdiği şeylerden biri salıncakta sallanmaktı.");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        yazi.GetComponent<TextMeshProUGUI>().SetText("Fakat o lanet olasıca gün... Keşke o gün parka gitmeseydik. O adamın bizi takip ettiğini farkettiğimde artık çok geçti. Yine öfkeli bir şekilde “Ne zaman bitecek?” diye sayıklıyordu.");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        yazi.GetComponent<TextMeshProUGUI>().SetText("Neden böyle konuştuğunu bilmiyordum. Fakat suçlu ne dondurmaydı ne de park... O adam benden hayallerimi, kızımı, her şeyimi çaldı!");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        ustAna.SetActive(false);
    }
    IEnumerator scene2Hikaye(){
        ustAna.SetActive(true);
        yazi.GetComponent<TextMeshProUGUI>().SetText("Güzel Emily, canım kızım, onu en son bisikletinin üstünde görmüştüm. Parkta sıkılınca “Baba, bisikletle gezebilir miyim?” dediğinde keşke olacakları görüp, “Hayır, eve gidiyoruz” deseydim fakat demedim...");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        yazi.GetComponent<TextMeshProUGUI>().SetText("Onun yerine “Tamam sen gitmeye başla, bende arkandayım” dedim. Ve yine o lanet olası adamı gördüm, kızımın arkasından koşuyordu.");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        yazi.GetComponent<TextMeshProUGUI>().SetText("Bu sefer “Kimsin sen?” diyecektim fakat... Fakat... (Ağlar) Yapamadım... Her şey için çok geçti...");
        yield return new WaitUntil(() => mouseClicked);
        mouseClicked=false;
        ustAna.SetActive(false);
    }
}
