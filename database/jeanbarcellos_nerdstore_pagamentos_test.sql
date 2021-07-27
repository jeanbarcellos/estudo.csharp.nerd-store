/*
 Navicat Premium Data Transfer

 Source Server         : pg_localhost
 Source Server Type    : PostgreSQL
 Source Server Version : 110004
 Source Host           : localhost:5433
 Source Catalog        : jeanbarcellos_nerdstore_pagamentos
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 110004
 File Encoding         : 65001

 Date: 03/07/2021 14:01:49
*/


-- ----------------------------
-- Table structure for Pagamentos
-- ----------------------------
DROP TABLE IF EXISTS "public"."Pagamentos";
CREATE TABLE "public"."Pagamentos" (
  "Id" uuid NOT NULL,
  "PedidoId" uuid NOT NULL,
  "Status" varchar(100) COLLATE "pg_catalog"."default",
  "Valor" numeric NOT NULL,
  "NomeCartao" varchar(100) COLLATE "pg_catalog"."default",
  "NumeroCartao" varchar(100) COLLATE "pg_catalog"."default",
  "ExpiracaoCartao" varchar(100) COLLATE "pg_catalog"."default",
  "CVVCartao" varchar(100) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of Pagamentos
-- ----------------------------
INSERT INTO "public"."Pagamentos" VALUES ('155a7823-1731-408e-b1b4-20db84fd5d2b', '62dc7e03-aaa7-4deb-84e7-a8ffa1391748', NULL, 165.00, 'Jean Barcellos', '1111-2222-3333-4444', '21/23', '122');
INSERT INTO "public"."Pagamentos" VALUES ('fe6a2a27-0417-48ad-b784-181961534fa5', 'b69c4647-3d0d-47e5-8bd2-5566153df1d1', NULL, 184.50, 'Nestor Cassin', '1111-2222-3333-4444', '10/25', '123');

-- ----------------------------
-- Table structure for Transacoes
-- ----------------------------
DROP TABLE IF EXISTS "public"."Transacoes";
CREATE TABLE "public"."Transacoes" (
  "Id" uuid NOT NULL,
  "PedidoId" uuid NOT NULL,
  "PagamentoId" uuid NOT NULL,
  "Total" numeric NOT NULL,
  "StatusTransacao" int4 NOT NULL
)
;

-- ----------------------------
-- Records of Transacoes
-- ----------------------------
INSERT INTO "public"."Transacoes" VALUES ('82978d22-801a-468c-be64-b5f2f2ac41ca', '62dc7e03-aaa7-4deb-84e7-a8ffa1391748', '155a7823-1731-408e-b1b4-20db84fd5d2b', 165.00, 1);
INSERT INTO "public"."Transacoes" VALUES ('2f9643c0-29fc-4fdc-8d02-de694793b2b8', 'b69c4647-3d0d-47e5-8bd2-5566153df1d1', 'fe6a2a27-0417-48ad-b784-181961534fa5', 184.50, 1);

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
INSERT INTO "public"."__EFMigrationsHistory" VALUES ('20210615002812_initial', '5.0.6');

-- ----------------------------
-- Primary Key structure for table Pagamentos
-- ----------------------------
ALTER TABLE "public"."Pagamentos" ADD CONSTRAINT "PK_Pagamentos" PRIMARY KEY ("Id");

-- ----------------------------
-- Indexes structure for table Transacoes
-- ----------------------------
CREATE UNIQUE INDEX "IX_Transacoes_PagamentoId" ON "public"."Transacoes" USING btree (
  "PagamentoId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table Transacoes
-- ----------------------------
ALTER TABLE "public"."Transacoes" ADD CONSTRAINT "PK_Transacoes" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table __EFMigrationsHistory
-- ----------------------------
ALTER TABLE "public"."__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");

-- ----------------------------
-- Foreign Keys structure for table Transacoes
-- ----------------------------
ALTER TABLE "public"."Transacoes" ADD CONSTRAINT "FK_Transacoes_Pagamentos_PagamentoId" FOREIGN KEY ("PagamentoId") REFERENCES "public"."Pagamentos" ("Id") ON DELETE RESTRICT ON UPDATE NO ACTION;
