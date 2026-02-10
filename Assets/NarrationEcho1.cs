using UnityEngine;
using UnityEngine.SceneManagement;

public class NarrationEcho1 : MonoBehaviour
{
    [SerializeField] private Transform cameraAlex;
    [SerializeField] private Transform cameraCollegue;
    [SerializeField] private Transform cameraTV;
    [SerializeField] private Transform cameraARIA;

    private void Start()
    {
        NarrationPart[] scene = new NarrationPart[]
        {
            new NarrationPart("Narrateur", new[] {
            "Une cuisine. L'odeur du pain grillé et du café bon marché.",
            "De la vraie lumière du soleil filtre par la fenêtre.",
            "Ton téléphone vibre : Loyer dû dans 3 jours."
            }, cameraAlex),

            new NarrationPart("Alex", new[] {
                "Ces mains... elles sont les miennes, mais plus douces.",
                "Un autocollant sur ma montre : « There is no Planet B ».",
                "Un souvenir d'une marche pour le climat où j'y étais surtout pour les photos."
            }),

            new NarrationPart("Télévision", new[] {
                "« ...trois nouvelles nations annoncent leur retrait des Accords de Paris... »",
                "« ...citant des pressions économiques et des priorités nationales... »"
            }, cameraTV),

            new NarrationPart("Alex", new[] {
                "Deux sacs devant la porte. Recyclage... et le reste.",
                "Un emballage gras dans la main. La poubelle de tri est juste là."
            },
            "Le jeter au recyclage.",
            new NarrationPart[] {
                new NarrationPart("Alex", new[] {
                    "Un petit geste. Dérisoire, peut-être.",
                    "Mais au moins celui-là, je l'ai fait."
                }, cameraAlex)
            },
            "Le jeter à la poubelle normale.",
            new NarrationPart[] {
                new NarrationPart("Alex", new[] {
                    "« Ça finit au même endroit de toute façon. »",
                    "C'est ce qu'un collègue m'a dit une fois. C'est resté parce que c'était plus facile."
                }, cameraAlex)
            }),

            new NarrationPart("Mme Ramos", new[] {
                "On est en octobre... Cette chaleur n'est pas normale." }),

            new NarrationPart("Alex", new[] {
                "Clé de voiture dans une main. Carte de transport dans l'autre.",
                "Le bus, c'est quinze minutes de plus. Quinze minutes pour... quelque chose."
            },
            "Prendre le bus.",
            new NarrationPart[] {
                new NarrationPart("Alex", new[] {
                    "Quinze minutes de plus. Le monde ne s'en souviendra pas.",
                    "Mais moi, peut-être que si."
                }, cameraAlex)
            },
            "Prendre la voiture.",
            new NarrationPart[] {
                new NarrationPart("Alex", new[] {
                    "« On ne peut pas sauver le monde si on ne peut même pas garder son boulot. »",
                    "La voiture se déverrouille avec un bip joyeux."
                }, cameraAlex)
            }),

            new NarrationPart("Collègue", new[] {
                "Hé Alex, t'as vu le nouveau rapport climat ?",
                "Un truc qui dit qu'on a largement dépassé la zone de sécurité."
            }, cameraCollegue),

            new NarrationPart("Alex", new[] {
                "« Ouais, mais ils disent ça depuis des années, non ? »",
                "Il rit, soulagé que je n'aie pas rendu ça gênant."
            }, cameraAlex),

            new NarrationPart("Alex", new[] {
                "Ce soir, sur le balcon. Le ciel est violet, étouffé par la pollution.",
                "Pour la première fois, je laisse la question se former : et si on ne répare jamais rien ?",
                "Un nom surgit de nulle part, comme un mot de passe créé dans une autre vie."
            }),

            new NarrationPart("Aria", new[] {
                "ARIA.",
                "Je suis de retour dans le désert. Ma main tremble encore contre le pilier.",
                "Ivan... mon nom est Aria."
            }, cameraARIA)
        };

        NarrationSystem.Instance.StartNarration(scene, () =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }
}
