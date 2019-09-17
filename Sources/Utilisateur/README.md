# Classe Connexion.cs

La classe connexion gère l'authentification de l'utilisateur au logiciel.
Son fonctionnement est assez simple, la requête paramétrée est refactorisée
en deux méthodes, l'une gère la requête et crée les paramètres.
La seconde exécute la requête en vérifiant si le nom d'utilisateur existe,
s'il existe, alors le mot de passe chiffré est stocké en variable,
déchiffré puis comparé au mot de passe entré par l'utilisateur
