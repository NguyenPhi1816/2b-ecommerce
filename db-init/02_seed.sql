BEGIN;

-- Dùng pgcrypto để hash mật khẩu (bcrypt)
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- 1) Upsert ROLE "ADMIN"
WITH upsert_role AS (
  INSERT INTO "Roles" ("Id","Name","Code")
  VALUES (uuid_generate_v4(), 'Admin', 'ADMIN')
  ON CONFLICT ("Code") DO UPDATE
    SET "Name" = EXCLUDED."Name"
  RETURNING "Id"
),
role_src AS (
  SELECT "Id" FROM upsert_role
  UNION ALL
  SELECT r."Id" FROM "Roles" r WHERE r."Code" = 'ADMIN' AND NOT EXISTS (SELECT 1 FROM upsert_role)
)
SELECT 1;

-- Lấy role id vào một biến tạm (dùng DO block)
DO $$
DECLARE
  v_admin_role uuid;
  v_user_id    uuid;
BEGIN
  SELECT "Id" INTO v_admin_role FROM "Roles" WHERE "Code" = 'ADMIN';

  -- 2) Tạo tài khoản admin nếu chưa có
  IF NOT EXISTS (SELECT 1 FROM "Users" WHERE "Email" = 'admin@local.test') THEN
    v_user_id := uuid_generate_v4();

    INSERT INTO "Users" (
      "Id","Email","PasswordHash","FirstName","LastName",
      "Dob","Gender","PhoneNumber","IsActive","CreatedAt","RoleId"
    ) VALUES (
      v_user_id,
      'admin@local.test',
      -- Hash bcrypt cho 'Admin@123' (đổi lại sau nếu muốn)
      crypt('Admin@123', gen_salt('bf', 10)),
      'System','Admin',
      DATE '1990-01-01',
      'MALE'::"GenderType",
      '0000000000',
      TRUE,
      now(),
      v_admin_role
    );
  END IF;
END $$;

COMMIT;
