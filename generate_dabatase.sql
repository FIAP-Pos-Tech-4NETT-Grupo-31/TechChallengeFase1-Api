CREATE TABLE dbo.Contatos (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Nome nvarchar(100),
	Email nvarchar(200),
	Telefone nvarchar(20),
	DDD INT,
);

CREATE TABLE dbo.Regioes (
	DDD INT NOT NULL,
	UF char(2)
);


select * from dbo.Regioes r;
SELECT * from dbo.Contatos c;


insert into dbo.Regioes values 
(21, 'RJ'),
(27, 'ES'),
(11, 'SP'),
(31, 'MG');



 