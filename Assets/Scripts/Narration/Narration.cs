using System.Collections.Generic;

public static partial class Narration
{
    public static NarrationPart[] Act1_Reveil = new NarrationPart[]
    {
        new NarrationPart("???", new[] { "Sujet conscient. Signes vitaux en stabilisation." }),

        new NarrationPart("Ivan", new[] {
            "Désignation : 1V4N. Tu peux m'appeler Ivan.",
            "Est-ce que tu te souviens de ton nom ?"
        }),

        new NarrationPart("???", new[] {
            "Je... je ne sais pas.",
            "Ma voix me semble étrangère. Mécanique, et pourtant... vivante."
        }),

        new NarrationPart("Ivan", new[] {
            "Dégradation mémorielle liée au cryosommeil prolongé. C'est documenté.",
            "J'ai localisé des structures qui pourraient aider. D'anciens piliers de données.",
            "Ils contiennent des schémas de résonance neurale compatibles avec ton architecture."
        }),

        new NarrationPart("???", new[] { "Où est tout le monde ?" }),

        new NarrationPart("Ivan", new[] {
            "Partis.",
            "Environ quatre-vingt-sept ans depuis la dernière transmission humaine dans cette région."
        }),

        new NarrationPart("???", new[] {
            "Le ciel est cuivré. Les arbres sont blancs comme des os.",
            "La terre est fissurée à perte de vue... Qu'est-ce qui s'est passé ici ?"
        }),

        new NarrationPart("Ivan", new[] {
            "Les réponses se trouvent dans les piliers.",
            "Le premier n'est pas loin. Es-tu prêt à voir ?"
        },
        "Allons-y, je veux comprendre.",
        new NarrationPart[] {
            new NarrationPart("Ivan", new[] { "En route. Prépare-toi... les souvenirs peuvent être déstabilisants." })
        },
        "J'ai besoin d'un moment.",
        new NarrationPart[] {
            new NarrationPart("Ivan", new[] {
                "Compris. Mais le temps n'est pas un luxe que nous avons.",
                "Quand tu seras prêt, le pilier nous attend."
            })
        }),

        new NarrationPart("???", new[] {
            "Un obélisque noir se dresse devant moi, parcouru de circuits émeraude.",
            "Quelque chose m'attire vers lui... comme si ces souvenirs m'appartenaient."
        }),

        new NarrationPart("Ivan", new[] {
            "Contact détecté. Transfert de résonance neurale en cours.",
            "Accroche-toi."
        })
    };
}