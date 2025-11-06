# 📘 GlossaireQuest

**GlossaireQuest** est une application web moderne développée en **Angular** (front-end) et **ASP.NET Core (C#)** (back-end). Elle permet aux utilisateurs de **répondre à des quiz interactifs** sur des thèmes pédagogiques variés.

---

## 🚀 Fonctionnalités principales

* ✅ Authentification des utilisateurs (login / inscription)
* 🧠 Création et gestion de quiz (par les administrateurs)
* 🗂️ Participation aux quiz avec système de score
* 📊 Suivi des résultats et statistiques par utilisateur
* 🧩 Interface moderne et responsive grâce à **Tailwind CSS**
* 🔒 Sécurité assurée via **JWT (JSON Web Token)**

---

## 🏗️ Architecture du projet

### Front-end — Angular

* Framework : **Angular 17**
* Styles : **Tailwind CSS** + composants personnalisés
* Routing : gestion des routes protégées (guard + AuthService)
* Services : communication REST avec l’API ASP.NET Core

### Back-end — ASP.NET Core

* Framework : **.NET 8 / ASP.NET Web API**
* ORM : **Entity Framework Core**
* Base de données : **PostgreSQL**
* Sécurité : **JWT Authentication**
* Organisation : Controllers / Services / Models / DTOs

---

## ⚙️ Installation et exécution

### Prérequis

* **Node.js** ≥ 18
* **.NET SDK** ≥ 8.0
* **PostgreSQL** (ou autre SGBD compatible EF Core)

### Étapes

#### 1️⃣ Cloner le projet

```bash
git clone https://github.com/LINDECKER-Charles/GlossaireQuest.git
cd GlossaireQuest
```

#### 2️⃣ Lancer le back-end

```bash
cd backend

dotnet restore
dotnet run
```

Par défaut, l’API tourne sur `https://localhost:5001`.

#### 3️⃣ Lancer le front-end

```bash
cd frontend

npm install
npm start
```

Accessible via `http://localhost:4200`.

---

## 🧩 Structure du dépôt

```
GlossaireQuest/
├── backend/           # API ASP.NET Core (C#)
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   └── Program.cs
│
├── frontend/          # Application Angular
│   ├── src/
│   ├── app/
│   ├── assets/
│   └── tailwind.config.js
│
└── README.md          # Documentation principale
```

---

## 🧑‍💻 Auteur

Projet développé par **Charles LINDECKER** — Full Stack Developer (Angular / .NET / Symfony).

🔗 GitHub : [LINDECKER-Charles](https://github.com/LINDECKER-Charles)

---

## 📄 Licence

Ce projet est distribué sous licence **MIT** — libre d’utilisation et de modification.
