# Conteúdo

Este repositório contem a implementação de uma API em .Net Core 8 e Dapper para atender minimamente os requisitos do Tech Challenge. Além do Dapper em si é utilizada também a biblioteca Dapper.Contrib.  O arquivo **"Contatos.dacpac"** é a definição do banco de dados que pode ser importada em qualquer SQL  Server.

### Swagger
![image](https://github.com/PauloAbinair/TechChallengeFase1-Api-Dapper/assets/40800755/7e7fdd96-36b0-4a30-9c11-a13536099efc)


### Tabelas

![image](https://github.com/PauloAbinair/TechChallengeFase1-Api-Dapper/assets/40800755/a00c480f-7842-4a92-a836-5dda438272c1)

![image](https://github.com/PauloAbinair/TechChallengeFase1-Api-Dapper/assets/40800755/bfae5110-be4d-4fcd-9969-c993d0c2b0b6)

# Tech Challenge

Tech Challenge é o projeto da fase que engloba os conhecimentos obtidos em todas as disciplinas dela. Esta é uma atividade que, a princípio, deve ser desenvolvida em grupo. É importante atentar-se ao prazo de entrega, uma vez que essa atividade é obrigatória e vale 60% da nota de todas as disciplinas da fase.

## O Problema

O Tech Challenge desta fase será desenvolver um aplicativo utilizando a plataforma .NET 8 para cadastro de contatos regionais, considerando a persistência de dados e a qualidade do software.

### Requisitos Funcionais

*   Cadastro de contatos: permitir o cadastro de novos contatos, incluindo nome, telefone e e-mail. Associe cada contato a um DDD correspondente à região.
    
*   Consulta de contatos: implementar uma funcionalidade para consultar e visualizar os contatos cadastrados, os quais podem ser filtrados pelo DDD da região.
    
*   Atualização e exclusão: possibilitar a atualização e exclusão de contatos previamente cadastrados.
    

### Requisitos Técnicos

*   Persistência de Dados: utilizar um banco de dados para armazenar as informações dos contatos. Escolha entre Entity Framework Core ou Dapper para a camada de acesso a dados.
    
*   Validações: implementar validações para garantir dados consistentes (por exemplo: validação de formato de e-mail, telefone, campos obrigatórios).
    
*   Testes Unitários: desenvolver testes unitários utilizando xUnit ou NUnit.
    

### Entrega

Para que possamos avaliar, esperamos um vídeo demonstrando os passos utilizados para o desenvolvimento do projeto, a arquitetura escolhida, o banco de dados e principalmente o projeto funcionando com os requisitos básicos.

### Observações

O foco principal é a qualidade do código, as boas práticas de desenvolvimento e o uso eficiente da plataforma .NET 8. Este desafio é uma oportunidade para demonstrar habilidades em persistência de dados, arquitetura de software e testes, além de boas práticas de desenvolvimento.

Ficou com alguma dúvida? Não deixe de nos chamar no Discord para que alguém da equipe te ajude!


### Grafana + Prometheus
Para iniciar os containeres, abrir o terminal no diretório do projeto e digitar os comandos
```
docker compose build
docker compose up
```

Com todos os containeres rodando, abrir o banco de dados e rodar o script inicial para ingestão de regiões.

Na url, localhost:9090 é possível acessar o grafana e monitorar o dashboard criado.