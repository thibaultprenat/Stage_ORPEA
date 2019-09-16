# Logiciel de gestion

La première année de BTS SIO Option SLAM impose d'effectuer un stage en entreprise d'une durée minimum de 5 semaines.
J'ai effectué ce stage au sein du groupe de maisons de retraite ORPEA à Saint-Etienne. Un cahier des charges m'a alors
été confié dans le but final d'obtenir un logiciel de gestion des résidents et des plannings.
Je vais donc dans un premier temps vous éclairer sur les différents choix à effectuer selon les besoins et les attentes (objectifs).
Dans un second temps, je vous présenterai les fonctionnalités incluses dans le logiciel.

## Pour commencer

![Logo OREPA Groupe](https://www.orpea-groupe.com/sites/default/files/styles/style_actualites_home/public/images_actualites/logo_orpea_groupe.jpg)

Avant tout codage, il était primordial de respecter une certaine hiérarchie afin de rester cohérent dans le processus de développement. Il m'a fallu réunir quelques éléments dont les suivants : 

* Environnement de développement
* Environnement de qualification
* Environnement de production

## Prérequis

Environnement de développement : 

| Support | IDE - Languages |Base de données - FTP|
|----------|:-------------:|------:|
| PC Portable professionnel (Win 10 Pro) | Visual Studio 2017 - C# (.NET WinForm) - SQL | MySQL (Outil : Navicat Premium) - FTP (Outil : FileZilla Client) |

Environnement de qualification : 

| Support | OS |
|----------|:-------------:|
| VM (VMware / VirtualBox) | Windows 7 - 10 (x32 / x64) |


Environnement de production : 

| Support | OS |
|----------|:-------------:|
| PC Portable / Bureau | Windows 7 - 10 (x32 / x64) |

# Pour qui ?

* Le logiciel est exclusivement conçu pour les salariés.

# Les fonctionnalités

Il est l'heure de nous intéresser aux différentes fonctionalités que propose ce logiciel. En premier lieu, je vais soumettre les fonctionnalités principales. Enfin, nous allons découvrir les fonctionnalités un peu plus techniques.

## Trois catégories distinctes...

### Gestion

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/categGestion.png) ![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmDesaffect.PNG) 

* **Désaffectation**

```
Un résident désaffecté est catalogué et disparaît de toutes les listes. Trois possibilités de désaffectation :
* Changement d'établissement
* Hospitalisation
* Décès
Dans le cas où un résident décède, toutes ses données sont alors automatiquement supprimées.
```

* **Réaffectation**

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmReaffect.PNG)

```
Tout naturellement, un résident désaffecté peut être réaffecté, 
il apparaîtra de nouveau dans toutes les listes (sauf exception cité ci-dessus).
```

* **Affectation**

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmAffect.PNG)

```
Lorsqu'un résident n'est pas renseigné dans la base de données (nouveau résident), 
il est nécessaire de créer une "Fiche résident". Cette fiche renseigne toutes les 
informations personnelles du résident (Nom, prénom, métiers, enfants, statut social etc...) 
ainsi que tous les centres d'intérêt de ce dernier. Les centres d'intérêt correspondent 
aux activités disponibles dans le catalogue de la maison de retraite. Pour chaque activité 
il est nécessaire d'indiquer si le résident est intéressé ou non, s'il a déjà pratiqué ou s'il n'est pas intéressé.
```

* **Historique**

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmHistorique.PNG)

```
Il est possible de consulter l'historique de chaque résident. Ce dernier recense : 
* Les informations personnelles du résident (avec possibilité de modifier)
* Les activités auxquelles le résident a participé
* Toutes les entrées du résident
* Toutes les sorties du résident
* Tous les centres d'intérêt du résident
```

### Planification

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/categPlanification.png) ![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmCreaAct.PNG)

* **Créer une activité**

```
Permet d'ajouter des activités aux catégories présentes dans la base de données
(6 catégories au total)
```

* **Planifier une activité**

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmPlanifAct.PNG)

```
Permet d'ajouter une activité au planning (Activité, catégorie, date)
```

### Activités

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/categActivites.png) ![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmPlanning.PNG)

* **Consulter le planning**

```
Consultation du planning des activités triées par date (De la plus récente à la plus ancienne) 
avec liste des participants.
```

* **Assigner les résidents**

![Environnement Dev](https://raw.githubusercontent.com/BidaultMathis/Stage_ORPEA/master/Images/frmAssign_censored.jpg)

```
Ce formulaire récupère les activités du planning en fonction des dates, la liste des résidents ainsi
que les résidents déjà affectés aux activités. Il est possible de retirer un résident d'une activité
simplement en cliquant sur le bouton "Supprimer" puis "Assigner".
```
