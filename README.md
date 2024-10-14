# Sistema de Gestão de Contas Bancárias

## Visão Geral

Este projeto implementa um sistema de gestão de contas bancárias utilizando a arquitetura limpa em uma API .NET 6. A aplicação permite realizar operações de débito, crédito e transferência, garantindo a integridade dos dados e a atomicidade das transações.

## Arquitetura

A arquitetura do sistema segue os princípios SOLID e utiliza os padrões Unit of Work, Repository e Command para promover a modularidade e a manutenção do código.

A API é composta por diferentes camadas que trabalham em conjunto para realizar as operações bancárias:

API: Ponto de entrada da aplicação, recebendo requisições HTTP e retornando respostas.
Serviço (Service): Camada de negócios responsável pela lógica das operações bancárias. Delega as operações para o repositório e gerencia as transações.
Repositório (Repository): Camada de acesso a dados, responsável por interagir com o banco de dados.
Unidade de Trabalho (Unit of Work): Gerencia as transações no banco de dados, garantindo a atomicidade das operações.
Modelo (Model): Camada que representa as entidades do domínio, como ContaBancaria e LancamentoBancario.
DTO (Data Transfer Object): Objetos utilizados para transferir dados entre camadas da aplicação.


### Classes do Modelo

ContaBancaria: Representa uma conta bancária, com atributos como identificador, nome e saldo.
LancamentoBancario: Representa uma transação bancária, com atributos como identificador, valor, tipo de operação (crédito, débito ou transferência), data de criação e contas bancárias de origem e destino.

### Descrição das Camadas

* **Apresentação:** A camada de apresentação é composta pelos controladores, que expõem a API REST e recebem as requisições HTTP.
* **Domínio:** A camada de domínio contém as entidades que representam os conceitos do negócio, como ContaBancaria e Transacao.
* **Infraestrutura:** A camada de infraestrutura contém os repositórios, que são responsáveis por acessar o banco de dados, e o Unit of Work, que gerencia as transações.

### Fluxos de Trabalho

* **Realizar uma Transação:**
  1. O cliente envia uma requisição HTTP para o controlador correspondente (e.g., `POST /contas/{contaId}/debitar`).
  2. O controlador recebe a requisição, mapeia os dados para um objeto de comando (e.g., `DebitarCommand`).
  3. O comando é executado, validando os dados e invocando o serviço de domínio.
  4. O serviço de domínio realiza a operação de débito, atualizando a conta bancária e registrando a transação.
  5. O repositório persiste as alterações no banco de dados.
  6. O controlador retorna uma resposta HTTP indicando o resultado da operação.

## Tecnologias Utilizadas

* **.NET 6:** Framework de desenvolvimento para a criação da API.
* **[Entity Framework Core]:** ORM (Object-Relational Mapper) utilizado para mapeamento entre objetos e banco de dados.

## Boas Práticas

* **SOLID:** Os princípios SOLID foram aplicados para garantir um código mais coeso, desacoplado e fácil de manter.
* **Padrões de Projeto:** Os padrões Unit of Work, Repository e Command foram utilizados para promover a modularidade e a reutilização de código.

