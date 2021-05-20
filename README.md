_Repositório apenas para estudo_

# NerdStore

Projeto desenvolvido no curso 'Modelagem de Domínios Ricos' juntamente com os testes feitos no curo 'Dominando Testes de Softwares'

Instrutor:

- [Eduardo Pires](https://www.eduardopires.net.br/)

Referências:

- https://desenvolvedor.io/curso-online-modelagem-de-dominios-ricos/
- https://desenvolvedor.io/curso-online-dominando-os-testes-de-software

## Modelatem Estratégica

### Requisitos

**Requisitos (Conversa com o):**

- A loja virtual exibirá um catálogo de produtos de diversas categorias
- Um cliente pode realizar um pedido contendo de 1 a N produtos.
- A loja realizará as vendas através de pagamento por cartão de crédito.
- O cliente irá realizar o seu cadastro para poder fazer pedidos.
- O cliente irá confirmar o pedido, endereço de entrega, escolher o tipo de frete e realizar o pagamento.
- Após o pagamento o pedido mudará de status conforme reposta da transação via cartão
- O correta a emissão da nota fiscal logo após a confirmação do pagamento do pedido

**Identificação dos elementos:**

- A loja virtual exibirá um **catálogo** de **produtos** de diversas **categorias**
- Um **cliente** pode realizar um **pedido** contendo de 1 a N produtos.
- A loja realizará as **vendas** através de **pagamento** por **cartão de crédito**.
- O cliente irá realizar o seu **cadastro** para poder fazer pedidos.
- O cliente irá confirmar o pedido, **endereço** de entrega, escolher o tipo de **frete** e realizar o pagamento.
- Após o pagamento o pedido mudará de **status** conforme reposta da **transação** via cartão
- O correta a emissão da **nota fiscal** logo após a confirmação do pagamento do pedido

**Identificação das possíveis ações:**

- A loja virtual **exibirá um catálogo de produtos de diversas categorias**
- Um cliente pode **realizar um pedido** contendo de 1 a N produtos.
- A loja **realizará as vendas** através de **pagamento por cartão de crédito**.
- O cliente irá **realizar o seu cadastro** para poder fazer pedidos.
- O cliente irá **confirmar o pedido**, endereço de entrega, **escolher o tipo de frete** e **realizar o pagamento**.
- Após o pagamento o **pedido mudará de status** conforme **reposta da transação via cartão**
- O correta a **emissão da nota fiscal** logo após a **confirmação do pagamento** do pedido

### Definindo contextos e elementos chave

![Contextos e elementos chave](https://github.com/jeanbarcellos/estudo.csharp.nerd-store/blob/master/docs/modeling/01%20Defininção%20dos%20contextos%20e%20elementos%20chave.jpg?raw=true)

### Enquadramento dos elementos dentro das categorias

![Contextos e elementos chave](https://github.com/jeanbarcellos/estudo.csharp.nerd-store/blob/master/docs/modeling/02%20Defininção%20dos%20contextos%20e%20elementos%20chave.jpg?raw=true)

_Tipos de Domínio:_

- Domínio Principal
- Domínio de Suporte
- Domínio Genérico

## Mapa de contexto

![Mapa de contexto](https://github.com/jeanbarcellos/estudo.csharp.nerd-store/blob/master/docs/modeling/03%20Mapa%20de%20Contexto%20e%20Relacionamentos.jpg?raw=true)

**Tipo de relacionamento entre contextos:**

- **Cliente-Fornecedor (Customer-Supplier Development)**
  - Onde um contexto influencia no ouro
- **Parceiro (Partner)**
  - Colaboram, não existe uma direção. Os dois são iguais
  - Se um muda, o outro muda também.
- **Conformista (Conformist)**
  - É quando duas equipes possuem uma dependência mútua
  - Precisam, portanto, trabalhar juntos
- **Camada de anticorrupção (Anticorruption Layer)**
  - Nesse relacionamento a equipe downstream decide criar uma camada para proteger seu contexto das modificações upstream. É um típico cenário de sistemas legados
  - Garantir que tenha um único ponto de quebra
  - Fachada
- **Núcleo Compartilhado (Shared Kernel)**
  - Quando vários bounded context compartilham um mesmo domínio. Alterar significa que todas as equipes serão afetadas.
  - Classes base e Interfaces
  - PERIGO: Tudo que tiver no núcleo compartilhada, terá um forte acoplamento em todas os contextos

## DDD

![Mapa de contexto](https://github.com/jeanbarcellos/estudo.csharp.nerd-store/blob/master/docs/theory/07%20DDD%20-%20Proposta%20de%20Camadas.jpg?raw=true)

### **Camada de apresentação (Presentation Layer)**

- Faz o front com o usuário
- Pode ser MVC, WebApi, REST, Mobile, SPA
- Camada de Apresentação está duplamente ligada com a camada de aplicação

### **Camada de aplicação (Application Layer)**

- Não precisa, necessariamente, ser um projeto na arquitetura.
- Pode ser um controller, simplesmente

_Responsabilidade_

- Em qual camada deve ficar o código que formata os dados para a apresentação?
  - Se os dados vão ser exibidos em apresentação é lá que deve ser tratado”
  - Isso parece ser responsabilidade de negócio, deveria ser uma camada de negócios.
  - Dados são dados, é a base de dados que retorna as informações tratadas
- Envia informação para apresentação
  - Fornece dados prontos para consumo conforme necessidade de exibição
- Orquestra cações disparadas pelos elementos de apresentação (Espécie de Workflow)
  - Casos de uso da camada de apresentação
- Duplamente ligada com a camada de apresentação
  - Pode ser estendida ou duplicada quando um novo frontend é adicionado

### **Camada de Domínio (Domain Layer)**

**_Domain Model Pattern:_**

- **Modelos do domínio**
  - Entidade baseada em OOP
  - Modelos Funcionais
- **Guia para classes entidades**
  - Convenções do DDD
    - Factories
    - Value types
    - Private setters
  - Dados e comportamento

**_Serviços de Domínio_**

- Partes do domínio que não se encaixam em entidades existentes
- Classes que agrupam comportamentos diversos
  - Tipicamente trabalhando com diversas entidades
- Implementação de processos que:
  - Requerem acesso à persistência para ler e gravar
  - Requerem acesso a serviços externos

Caso seja implementado CRQS, esta camada terá também Comandos, ComandoHandlers, Events...

### **Camada de Infraestrutura (Infrastructure Layer)**

Em resumo, é uma super camada que agrega o ferramental

Dica: Isole os detalhes da camada de infra

Fazer uso de facades para isolar detalhes tecnológicos

## Modelatem Tática

### **Estrutura**

Diretórios lógicos da Solution

- **Services** - Ficará toda modelagem dos contextos
  - **Cadastros** – Contexto Auxiliar
  - **Catalogo** – Contexto Principal
  - _**Core** – Núcleo Compartilhado (Shared Kernel)_
  - **Fiscal** – Contexto Auxiliar
  - **Pagamentos** – Contexto Genérico
  - **Vendas** – Contexto Principal
- **WebApps** - Ficará as aplicações web
