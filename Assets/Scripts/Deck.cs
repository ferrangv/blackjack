using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage1;
    public Text probMessage2;
    public Text probMessage3;

    public int[] values = new int[52];
    int cardIndex = 0;

   
    private void Awake()
    {    
        InitCardValues();

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        
        int aux = 1;
         for (int i = 0; i < values.Length; i++)
        {
            if (aux > 13) aux = 1;
            if (aux > 10)
            {
                values[i] = 10;         
            }
            else
            {
                values[i] = aux;              
            }
            if (aux == 1) values[i] = 11;
            aux++;
        }
         
    }

    private void ShuffleCards()
    {
        int[] valuesAux = new int[52];
        Sprite[] facesAux = new Sprite[52];
        int aux = 0;

        for (int i = 0; i < 52; i++)
        {
            int valor = 0;
            while (valor == 0)
            {
                aux = Random.Range(0, 52);
                valor = values[aux];
            }
            values[aux] = 0;
            valuesAux[i] = valor;
            facesAux[i] = faces[aux];
        }

        values = valuesAux;
        faces = facesAux;
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();

            CardHand jugador = player.GetComponent<CardHand>();
            CardHand banca = dealer.GetComponent<CardHand>();
            if (jugador.points.Equals(21))
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                finalMessage.text = "HAS GANADO!!";
               
            }
            
            if (banca.points.Equals(21))
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                finalMessage.text = "HAS PERDIDO";            
            }
        }
    }

    private void CalculateProbabilities()
    {
        CardHand jugador = player.GetComponent<CardHand>();
        CardHand banca = dealer.GetComponent<CardHand>();
        float fav1 = 0;
        float fav2 = 0;
        float fav3 = 0;
        int count = 3;
        

        if (banca.cards.Count.Equals(2)) {
            foreach (int value in values)
            {
                if (banca.cards[1].GetComponent<CardModel>().value + value > jugador.points)
                {
                    if (banca.cards[0].GetComponent<CardModel>().value.Equals(value))
                    {
                        count--;
                    }
                    if(count != 0)
                    {
                        fav1++;
                    }
                    
                }
            }
        }
        probMessage1.text = "Probabilidad de que el dealer tenga mas puntos = " + fav1 / (52-cardIndex-1);


        if (banca.cards.Count.Equals(2))
        {
            foreach (int value in values)
            {
                if (jugador.points + value >= 17 && jugador.points + value <= 21)
                {
                    if (banca.cards[0].GetComponent<CardModel>().value.Equals(value))
                    {
                        count--;
                    }
                    if (count != 0)
                    {
                        fav2++;
                    }
                }
            }
        }
        probMessage2.text = "Probabilidad de obtener entre 17 y 21 = " + fav2 / (52 - cardIndex);
  

        if (banca.cards.Count.Equals(2))
        {
            foreach (int value in values)
            {
                if (jugador.points + value > 21)
                {
                    if (banca.cards[0].GetComponent<CardModel>().value.Equals(value))
                    {
                        count--;
                    }
                    if (count != 0)
                    {
                        fav3++;
                    }
                }
            }
        }   
        probMessage3.text = "Probabilidad de obtener mas de 21 = " + fav3 / (52 - cardIndex);

    }

    void PushDealer()
    {
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;
        CalculateProbabilities();
    }

    void PushPlayer()
    {
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        CardHand jugador = player.GetComponent<CardHand>();
        CardHand banca = dealer.GetComponent<CardHand>();

        if (cardIndex.Equals(4))
        {
            banca.cards[0].GetComponent<SpriteRenderer>().sprite = banca.cards[0].GetComponent<CardModel>().front;
        }

        PushPlayer();
        
        if (jugador.points > 21)
        {
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "HAS PERDIDO";
        }

    }

    public void Stand()
    {
        CardHand jugador = player.GetComponent<CardHand>();
        CardHand banca = dealer.GetComponent<CardHand>();

        if (cardIndex.Equals(4))
        {
            banca.cards[0].GetComponent<SpriteRenderer>().sprite = banca.cards[0].GetComponent<CardModel>().front;
        }

         while (banca.points <= 16)
        {
            PushDealer();
        }
        if (jugador.points > banca.points || banca.points > 21)
        {
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "HAS GANADO!!";
        }
        else {
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "HAS PERDIDO";

        }

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
        
    }
    
}
