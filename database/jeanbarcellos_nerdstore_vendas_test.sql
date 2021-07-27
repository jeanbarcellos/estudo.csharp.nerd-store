/*
 Navicat Premium Data Transfer

 Source Server         : pg_localhost
 Source Server Type    : PostgreSQL
 Source Server Version : 110004
 Source Host           : localhost:5433
 Source Catalog        : jeanbarcellos_nerdstore_vendas
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 110004
 File Encoding         : 65001

 Date: 03/07/2021 14:03:14
*/


-- ----------------------------
-- Sequence structure for MinhaSequencia
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."MinhaSequencia";
CREATE SEQUENCE "public"."MinhaSequencia" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1000
CACHE 1;

-- ----------------------------
-- Table structure for PedidoItens
-- ----------------------------
DROP TABLE IF EXISTS "public"."PedidoItens";
CREATE TABLE "public"."PedidoItens" (
  "Id" uuid NOT NULL,
  "PedidoId" uuid NOT NULL,
  "ProdutoId" uuid NOT NULL,
  "ProdutoNome" varchar(250) COLLATE "pg_catalog"."default" NOT NULL,
  "Quantidade" int4 NOT NULL,
  "ValorUnitario" numeric NOT NULL
)
;

-- ----------------------------
-- Records of PedidoItens
-- ----------------------------
INSERT INTO "public"."PedidoItens" VALUES ('d63c0e3e-8682-4e97-82b2-f2af91e6f0c6', '7be97dd1-57d2-4732-be50-dd94753f7a03', 'c498acc1-b622-4507-b3ff-64084325635e', 'Camiseta Code Life Preta', 10, 90);

-- ----------------------------
-- Table structure for Pedidos
-- ----------------------------
DROP TABLE IF EXISTS "public"."Pedidos";
CREATE TABLE "public"."Pedidos" (
  "Id" uuid NOT NULL,
  "Codigo" int4 NOT NULL DEFAULT nextval('"MinhaSequencia"'::regclass),
  "ClienteId" uuid NOT NULL,
  "VoucherId" uuid,
  "VoucherUtilizado" bool NOT NULL,
  "Desconto" numeric NOT NULL,
  "ValorTotal" numeric NOT NULL,
  "DataCadastro" timestamp(6) NOT NULL,
  "PedidoStatus" int4 NOT NULL
)
;

-- ----------------------------
-- Records of Pedidos
-- ----------------------------
INSERT INTO "public"."Pedidos" VALUES ('7be97dd1-57d2-4732-be50-dd94753f7a03', 1003, '2d183a4d-f448-4324-81f3-b9fffe3272ae', NULL, 'f', 0, 900, '2021-06-29 20:51:10.914588', 0);

-- ----------------------------
-- Table structure for Vouchers
-- ----------------------------
DROP TABLE IF EXISTS "public"."Vouchers";
CREATE TABLE "public"."Vouchers" (
  "Id" uuid NOT NULL,
  "Codigo" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "Percentual" numeric,
  "ValorDesconto" numeric,
  "Quantidade" int4 NOT NULL,
  "TipoDescontoVoucher" int4 NOT NULL,
  "DataCriacao" timestamp(6) NOT NULL,
  "DataUtilizacao" timestamp(6),
  "DataValidade" timestamp(6) NOT NULL,
  "Ativo" bool NOT NULL,
  "Utilizado" bool NOT NULL
)
;

-- ----------------------------
-- Records of Vouchers
-- ----------------------------
INSERT INTO "public"."Vouchers" VALUES ('20bc1b93-5111-4d32-ab3e-779222bbdff9', 'PROMO-10-OFF', 10.00, NULL, 50, 0, '2021-02-01 00:00:00', NULL, '2021-12-31 00:00:00', 't', 'f');
INSERT INTO "public"."Vouchers" VALUES ('3ccbaf0b-9793-42cf-a27c-6f51c93e95d6', 'PROMO-15-REAIS', NULL, 15.00, 50, 1, '2021-02-01 00:00:00', NULL, '2021-12-31 00:00:00', 't', 'f');

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
INSERT INTO "public"."__EFMigrationsHistory" VALUES ('20210603012414_Initial', '5.0.6');

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
SELECT setval('"public"."MinhaSequencia"', 1004, true);

-- ----------------------------
-- Indexes structure for table PedidoItens
-- ----------------------------
CREATE INDEX "IX_PedidoItens_PedidoId" ON "public"."PedidoItens" USING btree (
  "PedidoId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table PedidoItens
-- ----------------------------
ALTER TABLE "public"."PedidoItens" ADD CONSTRAINT "PK_PedidoItens" PRIMARY KEY ("Id");

-- ----------------------------
-- Indexes structure for table Pedidos
-- ----------------------------
CREATE INDEX "IX_Pedidos_VoucherId" ON "public"."Pedidos" USING btree (
  "VoucherId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table Pedidos
-- ----------------------------
ALTER TABLE "public"."Pedidos" ADD CONSTRAINT "PK_Pedidos" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table Vouchers
-- ----------------------------
ALTER TABLE "public"."Vouchers" ADD CONSTRAINT "PK_Vouchers" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table __EFMigrationsHistory
-- ----------------------------
ALTER TABLE "public"."__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");

-- ----------------------------
-- Foreign Keys structure for table PedidoItens
-- ----------------------------
ALTER TABLE "public"."PedidoItens" ADD CONSTRAINT "FK_PedidoItens_Pedidos_PedidoId" FOREIGN KEY ("PedidoId") REFERENCES "public"."Pedidos" ("Id") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table Pedidos
-- ----------------------------
ALTER TABLE "public"."Pedidos" ADD CONSTRAINT "FK_Pedidos_Vouchers_VoucherId" FOREIGN KEY ("VoucherId") REFERENCES "public"."Vouchers" ("Id") ON DELETE RESTRICT ON UPDATE NO ACTION;
