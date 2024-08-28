--Create tables

CREATE TABLE public."Origins" (
	"Id" uuid NOT NULL,
	"Name" varchar(50) NOT NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NULL,
	"Ativo" bool NOT NULL,
	CONSTRAINT "PK_Origins" PRIMARY KEY ("Id")
);

CREATE TABLE public."TaskForces" (
	"Id" uuid NOT NULL,
	"Name" varchar(100) NOT NULL,
	"Priority" int4 NOT NULL,
	"Status" int4 NOT NULL,
	"Description" varchar(500) NOT NULL,
	"Assignee" varchar(100) NOT NULL,
	"OriginId" uuid NOT NULL,
	"CreatedAt" timestamptz NOT NULL,
	"UpdatedAt" timestamptz NULL,
	"Ativo" bool NOT NULL,
	CONSTRAINT "PK_TaskForces" PRIMARY KEY ("Id")
);
CREATE INDEX "IX_TaskForces_OriginId" ON public."TaskForces" USING btree ("OriginId");


ALTER TABLE public."TaskForces" ADD CONSTRAINT "FK_TaskForces_Origins_OriginId" FOREIGN KEY ("OriginId") REFERENCES public."Origins"("Id") ON DELETE RESTRICT;


--Insert origins

INSERT INTO public."Origins" ("Id", "Name", "CreatedAt", "UpdatedAt", "Ativo")
VALUES('02ae7bb4-aafd-4923-a3c4-ad67e979c3ce', 'Estratégico', CURRENT_DATE , null, true);

INSERT INTO public."Origins" ("Id", "Name", "CreatedAt", "UpdatedAt", "Ativo")
VALUES('f6e49039-8142-4222-b63f-fdbda7a6067e', 'Gerencial', CURRENT_DATE , null, true);

INSERT INTO public."Origins" ("Id", "Name", "CreatedAt", "UpdatedAt", "Ativo")
VALUES('d86a0087-09d7-46ae-a558-2b04a435f329', 'Operacional', CURRENT_DATE , null, true);