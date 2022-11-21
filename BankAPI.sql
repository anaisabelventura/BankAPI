-- DROP TABLE public.users CASCADE;
-- DROP TABLE public.accounts CASCADE;
-- DROP TABLE public.transfers CASCADE;

CREATE TABLE [public].[User] (
	id serial4 NOT NULL,
	username varchar NOT NULL,
	password varchar NOT NULL,
	full_name varchar NOT NULL,
	email varchar NOT NULL,
	--password_changed_at timestamptz NOT NULL DEFAULT now(),
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT users_email_key UNIQUE (email),
	CONSTRAINT users_pkey PRIMARY KEY (id),
	CONSTRAINT users_username UNIQUE (username),
);

CREATE TABLE [public].[Account] (
	id serial4 NOT NULL,
	user_id int NOT NULL,
	balance decimal NOT NULL,
	currency varchar NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT accounts_pkey PRIMARY KEY (id),
	CONSTRAINT accounts_fkey FOREIGN KEY(user_id) REFERENCES users(id)
);



CREATE TABLE [public].[Transfer] (
	id serial4 NOT NULL,
	from_account_id int NOT NULL,
	to_account_id int NOT NULL,
	amount decimal NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT transfers_pkey PRIMARY KEY (id),
	CONSTRAINT transfers_fkey FOREIGN KEY(from_account_id) REFERENCES accounts(id),
	CONSTRAINT transfers_fkey2 FOREIGN KEY(to_account_id) REFERENCES accounts(id)
);

/*CREATE TABLE public.operations_log (
	id serial4 NOT NULL,
	data json NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT transfers_pkey PRIMARY KEY (id),
);*/
