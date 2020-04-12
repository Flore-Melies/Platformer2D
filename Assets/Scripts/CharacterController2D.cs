using UnityEngine;
using UnityEngine.InputSystem; // Importer le namespace du nouvel Input System


public class CharacterController2D : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    
    private Controls myControls; // Déclarer une variable du type NomDuFichier des input actions
    private Vector2 inputDirection; // Sert à stocker la position du stick ou des flèches sur des axes [-1, 1]
    private Rigidbody2D myRB; // Déclaration d'une variable pour modifier le rigidbody ultérieurement
    private Animator myAnimator;
    
    private void OnEnable()
    {
        myControls = new Controls(); // Assigner une nouvelle instance de la classe NomDuFichier des input actions à sa variable
        myControls.Enable(); // Activer les inputs de l'input action sélectionnée
        myControls.Player.Move.performed += OnMovePerformed; // On ajoute à la liste des actions à effectuer lors du pressage du bouton, la fonction OnMovePerformed
        myControls.Player.Move.canceled += OnMoveCanceled; // Pareil lorsque l'on relache le bouton
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        inputDirection = Vector2.zero;
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        inputDirection = obj.ReadValue<Vector2>(); // On récupère la valeur de la position du stick ou des flèches et on l'assigne à l'inputDirection
    }

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>(); // On récupère le Rigidbody 2D présent sur cet objet
        myAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// S'éxecute plus souvent que l'Update. Sert à gérer la physique.
    /// </summary>
    private void FixedUpdate()
    {
        var direction = Vector2.right * (inputDirection.x * speed); // On assigne à une nouvelle variable la direction en X dans laquelle le stick est orienté
        //myRB.velocity = new Vector2(direction.x, myRB.velocity.y); // On assigner à notre velocité la valeur de notre vitesse horizontale
        if (myRB.velocity.sqrMagnitude <= maxSpeed) // Si la vitesse au carré de notre objet est inférieure ou égale à la vitesse maximum souhaitée
        {
            myRB.AddForce(direction); // On ajoute une force en direction du vecteur calculé précédemment
        }

        if (inputDirection.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (inputDirection.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        
        var isRunning = inputDirection.x != 0;
        // if Optionnel (pour la lisibilité)
        /*if (myRB.velocity.sqrMagnitude > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }*/
        myAnimator.SetBool("IsRunning", isRunning);
    }
}
