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

## Contexto Catálogo

...

## Contexto Vendas

Requisitos:

### **PEDIDO - ITEM PEDIDO - VOUCHER**

Um **_item_** de um **_pedido_** representa um produto e pode conter mais de uma unidade
Independente da ação, um item precisa ser sempre valido:

- Possuir: Id e Nome do produto, quantidade entre 1 e 15 unidades, valor maior que 0

Um pedido enquanto não iniciado (processo de pagamento) está no estado de _rascunho_
e deve pertencer a um cliente.

1. **Adicionar Item**

   1. Ao adicionar um item é necessário calcular o valor total do pedido
   2. Se um item já está na lista então deve acrescer a quantidade do item no pedido
   3. O item deve ter entre 1 e 15 unidades do produto

2. **Atualizacao de Item**

   1. O item precisa estar na lista para ser atualizado
   2. Um item pode ser atualizado contendo mais ou menos unidades do que anteriormente
   3. Ao atualizar um item é necessário calcular o valor total do pedido
   4. Um item deve permanecer entre 1 e 15 unidades do produto

3. **Remoção de Item**
   1. O item precisa estar na lista para ser removido
   2. Ao remover um item é necessário calcular o valor total do pedido

Um **_voucher_** possui um código único e o desconto pode ser em percentual ou valor fixo
Usar uma flag indicando que um pedido teve um voucher de desconto aplicado e o valor do desconto gerado

4. **Aplicar voucher de desconto**
   1. O voucher só pode ser aplicado se estiver válido, para isto:
      1. Deve possuir um código
      2. A data de validade é superior a data atual
      3. O voucher está ativo
      4. O voucher possui quantidade disponivel
      5. Uma das formas de desconto devem estar preenchidas com valor acima de 0
   2. Calcular o desconto conforme tipo do voucher
      1. Voucher com desconto percentual
      2. Voucher com desconto em valores (reais)
   3. Quando o valor do desconto ultrapassa o total do pedido o pedido recebe o valor: 0
   4. Após a aplicação do voucher o desconto deve ser re-calculado após toda modificação da lista de itens do pedido

### **PEDIDO COMMANDS - HANDLER**

- O command handler de pedido irá manipular um command para cada intenção em relação ao pedido.
  Em todos os commandos manipulados devem ser verificados:

  - Se o command é válido
  - Se o pedido existe
  - Se o item do pedido existe

- Na alteração de estado do pedido:

  - Deve ser feita via repositório
  - Deve enviar um evento

1. AdicionarItemPedidoCommand:

   1. Verificar se é um pedido novo ou em andamento
   2. Verificar se o item já foi adicionado a lista
