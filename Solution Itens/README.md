# 4NETT - Tech Challenge - Grupo 31

Este repositório contem os projetos dos Tech Challenge fases 1, 2 e 3 do Grupo 31, composto por:

- Paulo Abinair Prado Silva
- Rodrigo Cezar Leão
- Sandro dos Santos Gonçalves Filho
- José Nilton Alves Teixeir Junio
- Rodrigo Ramos Babisque Lima

## Arquitetura da Solução

#### *Informações Gerais*:
```
- Banco de dados SQL Server
- Utilização da stack de monitoramento PLG (Prometheus, Loki e Grafana)
- Aplicações instrumentadas com Prometheus-net para envio de métricas ao Prometheus
- Aplicações com logs estruturados via Serilog para envio ao Loki
- Grafana com dashboards para monitoramento de métricas e logs

 * Aplicações => Serilog => Loki => Grafana
 * Aplicações => Prometheus-net => Prometheus => Grafana
 * Aplicação Microserviço => RabbitMQ => Aplicação de Persistência => SQL Server
```

####  Projetos
- **Contatos.API**: web api desenvolvida na Fase 1 que se conecta ao SQL Server com Dapper e implementa os endpoints CRUD de contatos. Implementa ainda *Bearer Authentication* e cache.

- **Contatos.CadastroService**: minimal (microserviço) que implementa o *POST* de criação de contatos através de uma fila do RabbitMQ.

- **Contatos.CadastroService**: minimal (microserviço) que implementa o *GET* para listar todos ou retornar contatos por ID. Não usa fila, se conectando diretamente ao SQL Server via Entity Framework.

- **Contatos.PersistenciaService**: serviço de background que consome as filas do RabbitMQ e escreve as alterações no SQL Server.


## Solution Itens 

### Pasta "dev-env"

Contém os arquivos de configuração do docker-compose para subir o ambiente de desenvolvimento.

- As aplicações estão configuradas (*appsettings.Development.json* e *launchSettings.json*) para rodar em modo debug no Visual Studio se comunicando com os containers do banco de dados SQL Server, do RabbitMQ, do Prometheus, do Grafana e Loki.

- A pasta *dashboards* contém o json referenciado pelo docker-compose para provisionar o dashboard de monitoramento no Grafana.

- A pasta scripts contém o script de criação do banco de dados SQL Server.

- O container mssqltools apenas executa o script de criação do banco de dados e finaliza.

### Pasta "prod-env"

----> PENDENTE => DOCUMENTAR QUANDO FOREM FINALIZADAS AS IMPLEMENTAÇÕES


## Como rodar ...
 
 > DICA 1: todas as senhas foram definidas como `Grupo#31`.

 > DICA 2: rodas as aplicações com http. O Prometheus rodando no docker não conseguirá pegar as métricas das aplicações rodando no host com HTTPS.

  - Abrir o terminal na pasta *dev-env* e rodar o comando `docker-compose up`
  - Abrir a solution no Visual Studio e rodar as aplicações. Pode ser usada a configuração "Multiple Startup Projects" para rodar todas as aplicações de uma vez."
  - Para parar o ambiente, rodar o comando `docker-compose down` no terminal.
  
 #### Caminho padrão dos serviços:
 - [Contatos.API => http://localhost:5280](http://localhost:5280)
 - [Contatos.CadastroService => http://localhost:5292](http://localhost:5292)
 - [Contatos.ConsultaService => http://localhost:5275](http://localhost:5275)
 - [Contatos.PersistenciaService => http://localhost:5102](http://localhost:5102)
 - [Grafana => http://localhost:3000](http://localhost:3000) (user: admin, pass: Grupo#31)
 - [RabbitMQ => http://localhost:15672](http://localhost:15672) (user: guest, pass: Grupo#31)
 - [Prometheus => http://localhost:9090](http://localhost:9090)