/*
 Navicat Premium Data Transfer

 Source Server         : pg_localhost
 Source Server Type    : PostgreSQL
 Source Server Version : 110004
 Source Host           : localhost:5433
 Source Catalog        : jeanbarcellos_nerdstore_catalogo
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 110004
 File Encoding         : 65001

 Date: 03/07/2021 13:57:16
*/


-- ----------------------------
-- Table structure for Categorias
-- ----------------------------
DROP TABLE IF EXISTS "public"."Categorias";
CREATE TABLE "public"."Categorias" (
  "Id" uuid NOT NULL,
  "Nome" varchar(250) COLLATE "pg_catalog"."default" NOT NULL,
  "Codigo" int4 NOT NULL
)
;

-- ----------------------------
-- Records of Categorias
-- ----------------------------
INSERT INTO "public"."Categorias" VALUES ('06867776-b52c-4a85-8815-14ee1552a559', 'Camisetas', 100);
INSERT INTO "public"."Categorias" VALUES ('18e5b82c-6c4c-42ef-ab9f-5f142ecd663e', 'Canecas', 101);
INSERT INTO "public"."Categorias" VALUES ('e55d2134-b9ae-4921-bf05-b084e29248e1', 'Adeviso', 102);

-- ----------------------------
-- Table structure for Produtos
-- ----------------------------
DROP TABLE IF EXISTS "public"."Produtos";
CREATE TABLE "public"."Produtos" (
  "Id" uuid NOT NULL,
  "CategoriaId" uuid NOT NULL,
  "Nome" varchar(250) COLLATE "pg_catalog"."default" NOT NULL,
  "Descricao" varchar(500) COLLATE "pg_catalog"."default" NOT NULL,
  "Ativo" bool NOT NULL,
  "Valor" numeric NOT NULL,
  "DataCadastro" timestamp(6) NOT NULL,
  "Imagem" varchar(250) COLLATE "pg_catalog"."default" NOT NULL,
  "QuantidadeEstoque" int4 NOT NULL,
  "Altura" int4,
  "Largura" int4,
  "Profundidade" int4
)
;

-- ----------------------------
-- Records of Produtos
-- ----------------------------
INSERT INTO "public"."Produtos" VALUES ('ac6deb27-e7c9-4169-99f9-787b28283573', '18e5b82c-6c4c-42ef-ab9f-5f142ecd663e', 'Caneca Programmer COde', 'Descrição', 't', 15, '2021-02-27 17:14:35.909087', 'caneca2.jpg', 100, 5, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('0a988a84-3a2e-41bb-b8d7-7fb16a48cad0', '18e5b82c-6c4c-42ef-ab9f-5f142ecd663e', 'Caneca Turn Coffe In Code', 'Descrição', 't', 15, '2021-02-27 17:10:21.116423', 'caneca3.jpg', 100, 5, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('399a4305-1172-433a-891b-e0ac3857cde0', '18e5b82c-6c4c-42ef-ab9f-5f142ecd663e', 'Caneca No Coffe No Code', 'Descrição', 't', 30, '2021-02-27 17:10:21.116423', 'caneca4.jpg', 100, 5, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('82e966fe-806e-4395-97af-20bcf4f31733', '06867776-b52c-4a85-8815-14ee1552a559', 'Camiseta Debugar Preta', 'Descrição', 't', 110, '2021-02-27 00:00:00', 'camiseta4.jpg', 100, 11, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('c498acc1-b622-4507-b3ff-64084325635e', '06867776-b52c-4a85-8815-14ee1552a559', 'Camiseta Code Life Preta', 'Descrição', 't', 90, '2021-02-27 00:00:00', 'camiseta2.jpg', 100, 5, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('06867776-b52c-4a85-8815-14ee1552a559', '06867776-b52c-4a85-8815-14ee1552a559', 'Camiseta Sofware Developer', 'Descrição', 't', 100, '2021-02-27 17:10:21.116423', 'camiseta1.jpg', 100, 5, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('09c0e6b1-4763-4c77-bb92-ee3f68f8995e', '06867776-b52c-4a85-8815-14ee1552a559', 'Camiseta Code Life Cinza', 'Descrição', 't', 80, '2021-02-27 17:08:52.913199', 'camiseta3.jpg', 100, 5, 5, 5);
INSERT INTO "public"."Produtos" VALUES ('55c0eb46-8eee-441d-afc6-a8ba3d2881df', '18e5b82c-6c4c-42ef-ab9f-5f142ecd663e', 'Caneca Start Bugs', 'Descrição', 't', 25, '2021-02-27 17:10:21.116423', 'caneca1.jpg', 100, 5, 5, 5);

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS "public"."__EFMigrationsHistory";
CREATE TABLE "public"."__EFMigrationsHistory" (
  "MigrationId" varchar(150) COLLATE "pg_catalog"."default" NOT NULL,
  "ProductVersion" varchar(32) COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------
INSERT INTO "public"."__EFMigrationsHistory" VALUES ('20210526013034_Initial', '5.0.6');

-- ----------------------------
-- Primary Key structure for table Categorias
-- ----------------------------
ALTER TABLE "public"."Categorias" ADD CONSTRAINT "PK_Categorias" PRIMARY KEY ("Id");

-- ----------------------------
-- Indexes structure for table Produtos
-- ----------------------------
CREATE INDEX "IX_Produtos_CategoriaId" ON "public"."Produtos" USING btree (
  "CategoriaId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table Produtos
-- ----------------------------
ALTER TABLE "public"."Produtos" ADD CONSTRAINT "PK_Produtos" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table __EFMigrationsHistory
-- ----------------------------
ALTER TABLE "public"."__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");

-- ----------------------------
-- Foreign Keys structure for table Produtos
-- ----------------------------
ALTER TABLE "public"."Produtos" ADD CONSTRAINT "FK_Produtos_Categorias_CategoriaId" FOREIGN KEY ("CategoriaId") REFERENCES "public"."Categorias" ("Id") ON DELETE CASCADE ON UPDATE NO ACTION;
