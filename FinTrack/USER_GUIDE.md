# FinTrack — User Guide
## Personal Finance Tracker | CSE 325 Group Project | BYU-Idaho
### Team: Nsikak Okon · Victor · Clement

---

## Getting Started

### 1. Create an Account
1. Open the app in your browser
2. Click **"Register"** on the login page
3. Enter your first name, last name, email address, and a password (minimum 6 characters)
4. Click **"Create Account"** — you will be logged in automatically

### 2. Log In
1. Go to the app URL
2. Enter your registered email and password
3. Click **"Sign In"**
4. You will be taken to your personal Dashboard

### 3. Log Out
- Click **"Sign Out"** at the bottom of the left sidebar at any time

---

## Dashboard
The Dashboard gives you a real-time overview of your finances.

| Section | What it shows |
|---------|--------------|
| Total Balance | Your income minus your expenses — your net position |
| Total Income | Sum of all income transactions |
| Total Expenses | Sum of all expense transactions |
| Recent Transactions | Your 5 most recent transactions |
| Spending by Category | A progress bar breakdown of where your money goes |

---

## Transactions
Manage all your income and expense records.

### Add a Transaction
1. Click **"Transactions"** in the sidebar
2. Click **"+ Add Transaction"**
3. Fill in the form:
   - **Type** — Income or Expense
   - **Amount** — the dollar amount
   - **Date** — when the transaction occurred
   - **Category** — choose from the pre-loaded categories
   - **Description** — a short note (e.g. "Grocery run", "Monthly salary")
4. Click **"Add"**

### Edit a Transaction
1. Hover over any transaction row
2. Click the ✏️ pencil icon
3. Update the fields and click **"Update"**

### Delete a Transaction
1. Hover over the transaction
2. Click the 🗑️ trash icon
3. Confirm the deletion in the popup

### Filter Transactions
Use the dropdown at the top of the page to filter by:
- All Types
- Income only
- Expenses only

---

## Savings Goals
Set financial targets and track your progress.

### Create a Goal
1. Click **"Savings Goals"** in the sidebar
2. Click **"+ New Goal"**
3. Fill in:
   - **Title** — e.g. "Emergency Fund", "New Laptop"
   - **Description** — optional note
   - **Target Amount** — how much you want to save
   - **Target Date** — your deadline
4. Click **"Create Goal"**

### Add Money to a Goal
1. Find the goal card
2. Type an amount in the deposit input field
3. Click **"Add"**
4. The progress bar updates automatically

### Goal Completion
When your saved amount reaches the target, the goal is automatically marked as **Completed ✓** and the deposit controls are hidden.

### Delete a Goal
Click the 🗑️ icon on any active goal card.

---

## Categories
FinTrack includes 10 pre-loaded categories — 7 expense and 3 income:

**Expense categories:** Food & Dining, Rent & Housing, Transport, Entertainment, Healthcare, Education, Shopping

**Income categories:** Salary, Freelance, Other Income

---

## Tips
- Add transactions as they happen so your dashboard stays accurate
- Use the spending chart on the Dashboard to spot where you overspend
- Set a savings goal for each major financial target you have
- All your data is private — other users cannot see your transactions or goals

---

## Technical Information
- **Frontend:** .NET 10 Blazor WebAssembly
- **Backend:** ASP.NET Core 10 Web API
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** ASP.NET Core Identity + JWT tokens
- **Deployed on:** Microsoft Azure App Service

---

*FinTrack — CSE 325: .NET Software Development | BYU-Idaho | 2025*
