using System.Collections.Generic;

public static class Narration
{
    public static NarrationPart[] IntroVillage = new NarrationPart[]
    {
        new NarrationPart("???", new[] { "Hé toi ! Réveille-toi !" }),
        new NarrationPart("Garde", new[] { "Tu ne peux pas dormir ici.", "Le village n'est pas un refuge pour vagabonds." }),
        new NarrationPart("Garde", new[] { "Allez, debout. Le chef veut te voir." })
    };

    public static NarrationPart[] ChefVillage = new NarrationPart[]
    {
        new NarrationPart("Chef Aldric", new[] { "Ainsi c'est toi l'étranger qu'on a trouvé près de la forêt.", "Tu as de la chance d'être encore en vie." }),
        new NarrationPart("Chef Aldric", new[] { "Les créatures qui rôdent là-bas n'épargnent personne." }),
        new NarrationPart("Chef Aldric",
            new[] { "Dis-moi, que faisais-tu dans cette forêt maudite ?" },
            "Je ne m'en souviens pas...",
            new NarrationPart[]
            {
                new NarrationPart("Chef Aldric", new[] { "Amnésie ? Intéressant...", "Ou peut-être mens-tu." }),
                new NarrationPart("Chef Aldric", new[] { "Peu importe. Tu me seras utile." })
            },
            "Ce ne sont pas vos affaires.",
            new NarrationPart[]
            {
                new NarrationPart("Chef Aldric", new[] { "Ho ho ! Du caractère !", "J'aime ça." }),
                new NarrationPart("Chef Aldric", new[] { "Mais ici, tout est mes affaires.", "Retiens-le bien." })
            }),
        new NarrationPart("Chef Aldric", new[] { "En attendant, tu peux rester au village.", "Parle à Mira à l'auberge, elle te trouvera un lit." })
    };

    public static NarrationPart[] Aubergiste = new NarrationPart[]
    {
        new NarrationPart("Mira", new[] { "Bienvenue à l'Auberge du Sanglier Doré !", "Aldric m'a prévenue de ton arrivée." }),
        new NarrationPart("Mira", new[] { "Ta chambre est en haut, première porte à gauche.", "Le repas est servi au coucher du soleil." }),
        new NarrationPart("Mira",
            new[] { "Tu as l'air épuisé. Un conseil ?" },
            "Oui, je vous écoute.",
            new NarrationPart[]
            {
                new NarrationPart("Mira", new[] { "Évite la forêt à l'est.", "Ceux qui y entrent... ne reviennent pas tous." }),
                new NarrationPart("Mira", new[] { "Et si tu croises un homme en cape noire, fuis.", "Ne pose pas de questions, fuis." })
            },
            "Non merci, je me débrouillerai.",
            new NarrationPart[]
            {
                new NarrationPart("Mira", new[] { "Comme tu voudras.", "Les têtus ne font pas long feu par ici..." }),
                new NarrationPart("Mira", new[] { "Mais je suppose que tu l'apprendras par toi-même." })
            })
    };

    public static NarrationPart[] MarchandAmbulant = new NarrationPart[]
    {
        new NarrationPart("Marchand", new[] { "Approche, approche !", "J'ai des merveilles venues des quatre coins du monde !" }),
        new NarrationPart("Marchand", new[] { "Potions, amulettes, cartes au trésor...", "Tout ce dont un aventurier a besoin !" }),
        new NarrationPart("Marchand", new[] { "Reviens me voir quand tu auras quelques pièces." })
    };

    public static NarrationPart[] EnfantPerdu = new NarrationPart[]
    {
        new NarrationPart("Petit Théo", new[] { "M-Monsieur... Madame...", "*snif*" }),
        new NarrationPart("Petit Théo", new[] { "J'ai perdu mon chien... Flocon...", "Il est parti vers la forêt..." }),
        new NarrationPart("Petit Théo",
            new[] { "Tu pourrais m'aider à le retrouver ? S'il te plaît ?" },
            "Bien sûr, je vais le chercher.",
            new NarrationPart[]
            {
                new NarrationPart("Petit Théo", new[] { "C'est vrai ?! Merci merci merci !", "Flocon est tout blanc avec une tache noire sur l'oreille !" }),
                new NarrationPart("Petit Théo", new[] { "Je t'attendrai ici ! Fais attention !" })
            },
            "Désolé, je n'ai pas le temps.",
            new NarrationPart[]
            {
                new NarrationPart("Petit Théo", new[] { "Oh...", "*les larmes coulent*" }),
                new NarrationPart("Petit Théo", new[] { "D-D'accord... Je comprends..." })
            })
    };
}