# microServies
Заметки - микросервисы на ASP + Phyton

<pre>
База данных - Mongo DB
Connection string 
mongodb+srv://<username>:<password>@test-projects-cluster.qqhcelg.mongodb.net/?retryWrites=true&w=majority&appName=test-projects-cluster

Data Base Name - test

Collenctions:

user_servers  - пользователи
category_servers - категории заметок
note_servers - заметки

</pre>

#ASP 
для храниения паролей и логина к БД 
<pre>
dotnet user-secrets init
dotnet user-secrets set "MongoDB:Username" "yourUsername"
dotnet user-secrets set "MongoDB:Password" "yourPassword"
</pre>