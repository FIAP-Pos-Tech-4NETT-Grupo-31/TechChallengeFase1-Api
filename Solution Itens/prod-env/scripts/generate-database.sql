-- Criação do banco de dados Contatos
CREATE DATABASE Contatos;
GO

-- Seleciona o banco de dados Contatos para uso
USE Contatos;
GO

-- Criação da tabela Regioes
CREATE TABLE [dbo].[REGIOES] (
    [DDD] INT      IDENTITY (1, 1) NOT NULL,
    [UF]  CHAR (2) NOT NULL,
    CONSTRAINT [PK_Regioes] PRIMARY KEY CLUSTERED ([DDD] ASC)
);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DDD da região', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REGIOES', @level2type = N'COLUMN', @level2name = N'DDD';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UF da região', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REGIOES', @level2type = N'COLUMN', @level2name = N'UF';
GO

-- Criação da tabela Contatos
CREATE TABLE [dbo].[CONTATOS] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Nome]     NVARCHAR (100) NOT NULL,
    [Email]    VARCHAR (200)  NOT NULL,
    [Telefone] NVARCHAR (20)  NOT NULL,
    [DDD]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contatos_ToRegioes] FOREIGN KEY ([DDD]) REFERENCES [dbo].[REGIOES] ([DDD])
);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código do contato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CONTATOS', @level2type = N'COLUMN', @level2name = N'Id';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nome do contato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CONTATOS', @level2type = N'COLUMN', @level2name = N'Nome';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Email do contato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CONTATOS', @level2type = N'COLUMN', @level2name = N'Email';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Telefone do contato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CONTATOS', @level2type = N'COLUMN', @level2name = N'Telefone';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DDD da região', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CONTATOS', @level2type = N'COLUMN', @level2name = N'DDD';
GO

-- Inserção de dados na tabela Regioes
SET IDENTITY_INSERT [dbo].[REGIOES] ON
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (11, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (12, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (13, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (14, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (15, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (16, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (17, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (18, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (19, N'SP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (21, N'RJ')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (22, N'RJ')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (24, N'RJ')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (27, N'ES')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (28, N'ES')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (31, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (32, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (33, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (34, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (35, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (37, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (38, N'MG')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (41, N'PR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (42, N'PR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (43, N'PR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (44, N'PR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (45, N'PR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (46, N'PR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (47, N'SC')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (48, N'SC')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (49, N'SC')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (51, N'RS')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (53, N'RS')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (54, N'RS')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (55, N'RS')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (61, N'DF')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (62, N'GO')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (63, N'TO')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (64, N'GO')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (65, N'MT')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (66, N'MT')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (67, N'MS')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (68, N'AC')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (69, N'RO')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (71, N'BA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (73, N'BA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (74, N'BA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (75, N'BA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (77, N'BA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (79, N'SE')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (81, N'PE')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (82, N'AL')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (83, N'PB')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (84, N'RN')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (85, N'CE')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (86, N'PI')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (87, N'PE')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (88, N'CE')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (89, N'PI')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (91, N'PA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (92, N'AM')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (93, N'PA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (94, N'PA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (95, N'RR')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (96, N'AP')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (97, N'AM')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (98, N'MA')
INSERT INTO [dbo].[REGIOES] ([DDD], [UF]) VALUES (99, N'MA')
SET IDENTITY_INSERT [dbo].[REGIOES] OFF
