🚀 JobPortal: Queue-Based Recruitment Ecosystem
JobPortal is a sophisticated ASP.NET Core MVC platform designed to modernize the recruitment workflow. Unlike standard job boards, this system implements Queue-Based Logic and Priority Handling to ensure that high-priority job roles and urgent applications are processed efficiently, reducing bottlenecks in organizational hiring.

🛠️ Core Innovation
Application Queuing: Submissions are ordered by submission time.

Real-time Tracking: Applicants receive immediate status updates via interactive dashboards and automated email notifications.

🌟 Key Features
🏢 Organization & Hiring Panel
Full CRUD Lifecycle: Post, edit, and manage job listings with specialized metadata (Salary, Vacancy, Tags).

Interactive Hiring Panel: A custom overlay interface to manage applicants without leaving the dashboard.

Status Workflow: One-click "Accept" or "Reject" logic with integrated SMTP Email Services to notify candidates.

👤 Applicant Experience
Smart Discovery: Multi-parameter search and filtering (Keyword + Job Type) to find relevant opportunities.

AJAX Bookmark System: A seamless "Save for Later" feature using asynchronous Fetch API calls.

Document Processing: Secure upload and storage of Resumes and Cover Letters using both physical file paths and Byte Array (BLOB) database storage.

🛡️ Admin Intelligence
Data Visualization: A comprehensive dashboard using Chart.js to track job posting trends and platform growth.

System Moderation: Oversight of all user roles, organizations, and application queues to ensure platform integrity.

🏗️ Technical Implementation
Backend: C# / ASP.NET Core MVC 10

Database: SQL Server managed via Entity Framework Core.

Frontend: Razor Views, Bootstrap, JavaScript.

State Management: Session-based authentication and role-based access control (RBAC).

Pattern: Repository-style data management within the MVC architecture.

📊 Database Architecture
The system utilizes a relational schema to manage complex dependencies:

Organizations ↔ Jobs: One-to-Many relationship.

Jobs ↔ Applicants: Linked via the Apply table for tracking status.

Users ↔ Jobs: Many-to-Many relationship via the Bookmark entity.

🚀 How to Run
Clone: git clone https://github.com/Lamia013/JobPortal.git

Configure: Update the JobPortalConnection string in appsettings.json.

Migrate: Run Update-Database in the Package Manager Console.

Execute: Press F5 in Visual Studio or dotnet watch run, or dotnet run, in VS Code