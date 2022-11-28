DROP TABLE IF EXISTS "Users";
DROP TABLE IF EXISTS "Account";

DROP TABLE IF EXISTS "Transfer";

CREATE TABLE "Users" (
	id serial4 NOT NULL,
	username varchar NOT NULL,
	password varchar NOT NULL,
	full_name varchar NOT NULL,
	email varchar NOT NULL,
	--password_changed_at timestamptz NOT NULL DEFAULT now(),
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT users_email_key UNIQUE (email),
	CONSTRAINT users_pkey PRIMARY KEY (id),
	CONSTRAINT users_username UNIQUE (username)
);

CREATE TABLE "Account" (
	id serial4 NOT NULL,
	user_id int NOT NULL,
	balance decimal NOT NULL,
	currency varchar NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT accounts_pkey PRIMARY KEY (id),
	CONSTRAINT accounts_fkey FOREIGN KEY(user_id) REFERENCES "Users" (id)
);

CREATE TABLE "Transfer" (
	id serial4 NOT NULL,
	senderaccountid int NOT NULL,
	destinationaccountid int NOT NULL,
	amount decimal NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT transfers_pkey PRIMARY KEY (id),
	CONSTRAINT transfers_fkey FOREIGN KEY(senderaccountid) REFERENCES "Account" (id),
	CONSTRAINT transfers_fkey2 FOREIGN KEY(destinationaccountid) REFERENCES "Account" (id)
);

/*CREATE TABLE public.operations_log (
	id serial4 NOT NULL,
	data json NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT transfers_pkey PRIMARY KEY (id),
);*/
