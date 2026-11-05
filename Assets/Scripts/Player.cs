using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    private InputAction moveInput;
    

    //public float force;
    public float speed;
    public float jumpPower;

    public Rigidbody2D myBody;

    private Vector2 inputForce;
    
    public Transform groundCheck;
    public float checkSize;
    public LayerMask groundLayer;
    bool onGround;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject championItem;
    private bool win = false;
    
    [SerializeField] private GameObject restartButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("move");
        winText.SetActive(false);
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        inputForce = moveInput.ReadValue<Vector2>();
        if (transform.position.y < -15f)
        {
            RestartGame();
        }
    }

    private void FixedUpdate()
    {
        //myBody.AddForce(inputForce * force);
        myBody.linearVelocityX = inputForce.x * speed;
        if (inputForce.y > 0 && onGround)// onGround is a bool
        {
            myBody.linearVelocityY = inputForce.y * jumpPower;
            onGround = false;
        }
        else
        {
            onGround = Physics2D.OverlapCircle(groundCheck.position, checkSize, groundLayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == championItem && !win)
        {
            win = true;
            winText.SetActive(true);
            championItem.SetActive(false);
            restartButton.SetActive(true);
        }
        
        Hazard hazard = other.GetComponentInParent<Hazard>();

        if (hazard != null)
        {
            RestartGame();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
