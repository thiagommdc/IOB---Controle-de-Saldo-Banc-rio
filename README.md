# Sistema de Gest�o de Contas Banc�rias

## Vis�o Geral

Este projeto implementa um sistema de gest�o de contas banc�rias utilizando a arquitetura limpa em uma API .NET 6. A aplica��o permite realizar opera��es de d�bito, cr�dito e transfer�ncia, garantindo a integridade dos dados e a atomicidade das transa��es.

## Arquitetura

A arquitetura do sistema segue os princ�pios SOLID e utiliza os padr�es Unit of Work, Repository e Command para promover a modularidade e a manuten��o do c�digo.

A API � composta por diferentes camadas que trabalham em conjunto para realizar as opera��es banc�rias:

API: Ponto de entrada da aplica��o, recebendo requisi��es HTTP e retornando respostas.
Servi�o (Service): Camada de neg�cios respons�vel pela l�gica das opera��es banc�rias. Delega as opera��es para o reposit�rio e gerencia as transa��es.
Reposit�rio (Repository): Camada de acesso a dados, respons�vel por interagir com o banco de dados.
Unidade de Trabalho (Unit of Work): Gerencia as transa��es no banco de dados, garantindo a atomicidade das opera��es.
Modelo (Model): Camada que representa as entidades do dom�nio, como ContaBancaria e LancamentoBancario.
DTO (Data Transfer Object): Objetos utilizados para transferir dados entre camadas da aplica��o.


### Classes do Modelo

ContaBancaria: Representa uma conta banc�ria, com atributos como identificador, nome e saldo.
LancamentoBancario: Representa uma transa��o banc�ria, com atributos como identificador, valor, tipo de opera��o (cr�dito, d�bito ou transfer�ncia), data de cria��o e contas banc�rias de origem e destino.

### Descri��o das Camadas

* **Apresenta��o:** A camada de apresenta��o � composta pelos controladores, que exp�em a API REST e recebem as requisi��es HTTP.
* **Dom�nio:** A camada de dom�nio cont�m as entidades que representam os conceitos do neg�cio, como ContaBancaria e Transacao.
* **Infraestrutura:** A camada de infraestrutura cont�m os reposit�rios, que s�o respons�veis por acessar o banco de dados, e o Unit of Work, que gerencia as transa��es.

### Fluxos de Trabalho

* **Realizar uma Transa��o:**
  1. O cliente envia uma requisi��o HTTP para o controlador correspondente (e.g., `POST /contas/{contaId}/debitar`).
  2. O controlador recebe a requisi��o, mapeia os dados para um objeto de comando (e.g., `DebitarCommand`).
  3. O comando � executado, validando os dados e invocando o servi�o de dom�nio.
  4. O servi�o de dom�nio realiza a opera��o de d�bito, atualizando a conta banc�ria e registrando a transa��o.
  5. O reposit�rio persiste as altera��es no banco de dados.
  6. O controlador retorna uma resposta HTTP indicando o resultado da opera��o.

## Tecnologias Utilizadas

* **.NET 6:** Framework de desenvolvimento para a cria��o da API.
* **[Entity Framework Core]:** ORM (Object-Relational Mapper) utilizado para mapeamento entre objetos e banco de dados.

## Boas Pr�ticas

* **SOLID:** Os princ�pios SOLID foram aplicados para garantir um c�digo mais coeso, desacoplado e f�cil de manter.
* **Padr�es de Projeto:** Os padr�es Unit of Work, Repository e Command foram utilizados para promover a modularidade e a reutiliza��o de c�digo.

