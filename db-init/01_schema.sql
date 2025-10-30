

-- ===================================================================
-- E-commerce Database Schema (PostgreSQL)
-- Generated: 2025-10-23 (UTC+7)
-- Notes:
-- - Requires the "uuid-ossp" extension for uuid_generate_v4().
-- - This script creates enums, tables, constraints, and indexes.
-- - Uses TEXT + CHECK for some statuses to allow future values.
-- ===================================================================


BEGIN;


-- ---------------------------------------------------
-- Extensions
-- ---------------------------------------------------
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


-- ---------------------------------------------------
-- Enums
-- ---------------------------------------------------
-- Inventory movement type
DO $$ BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'InventoryType') THEN
CREATE TYPE "InventoryType" AS ENUM ('IN','OUT','TRANSFER_IN','TRANSFER_OUT','ADJUST');
  END IF;
END $$;


-- Payment method
DO $$ BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'PaymentMethod') THEN
CREATE TYPE "PaymentMethod" AS ENUM ('COD','EWALLET');
  END IF;
END $$;


-- Discount mode
DO $$ BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'DiscountMode') THEN
CREATE TYPE "DiscountMode" AS ENUM ('PERCENT','AMOUNT');
  END IF;
END $$;


-- Order status
DO $$ BEGIN
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'OrderStatus') THEN
CREATE TYPE "OrderStatus" AS ENUM ('PENDING', 'CONFIRMED', 'SHIPPED', 'DELIVERED', 'CANCELLED', 'RETURNED');
  END IF;
END $$;

-- Payment Status
DO $$ BEGIN
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'PaymentStatus') THEN
CREATE TYPE "PaymentStatus" AS ENUM ('PENDING','PAID','FAILED','REFUNDED');
  END IF;
END $$;

-- Gender type
DO $$ BEGIN
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'GenderType') THEN
CREATE TYPE "GenderType" AS ENUM ('MALE','FEMALE');
  END IF;
END $$;


-- ---------------------------------------------------
-- Tables
-- ---------------------------------------------------


-- roles
CREATE TABLE IF NOT EXISTS "Roles" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Name"        text NOT NULL,
  "Code"        text NOT NULL UNIQUE);


-- permissions
CREATE TABLE IF NOT EXISTS "Permissions" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Resource"    text,
  "Action"      text);


-- role_permissions (junction)
CREATE TABLE IF NOT EXISTS "RolePermissions" (
  "Id"             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "RoleId"        uuid NOT NULL REFERENCES "Roles"("Id") ON DELETE CASCADE,
  "PermissionId"  uuid NOT NULL REFERENCES "Permissions"("Id") ON DELETE CASCADE,
  CONSTRAINT uq_role_permission UNIQUE ("RoleId", "PermissionId"));


-- users
CREATE TABLE IF NOT EXISTS "Users" (
  "Id"             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Email"          text NOT NULL UNIQUE,
  "PasswordHash"  text NOT NULL,
  "FirstName"     text NOT NULL,
  "LastName"      text NOT NULL,
  "Dob"            date NOT NULL,
  "Gender"         "GenderType" NOT NULL,
  "PhoneNumber"   text NOT NULL,
  "IsActive"      boolean NOT NULL DEFAULT true,
  "CreatedAt"     timestamptz NOT NULL DEFAULT now(),
  "RoleId"        uuid REFERENCES "Roles"("Id") ON DELETE SET NULL);


-- ranks
CREATE TABLE IF NOT EXISTS "Ranks" (
  "Id"               		uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Name"             		text NOT NULL,
  "MinOrderValue"  		numeric(18,2) NOT NULL,
  "MaxOrderValue"  		numeric(18,2) NOT NULL,
  "Color"            		text NOT NULL,
  "Icon"             		text NOT NULL,
  "CalculationWindow"  text NOT NULL, -- Ví dụ: LIFETIME: Từ lúc tạo tài khoản, ROLLING_12M: Tính từ 12 tháng gần nhất, ...
  "Description"      		text NOT NULL);


-- rank_users
CREATE TABLE IF NOT EXISTS "RankUsers" (
  "Id"             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "UserId"        uuid NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
  "RankId"        uuid NOT NULL REFERENCES "Ranks"("Id") ON DELETE RESTRICT,
  "TotalSpent"    numeric(18,2) NOT NULL,
  "TotalOrders"   int NOT NULL,
  "LastOrderAt"  timestamptz NOT NULL DEFAULT now(),
  "AchievedAt"    timestamptz NOT NULL DEFAULT now(),
  "ExpiredAt"     timestamptz NOT NULL DEFAULT now(),
  "IsActive"      boolean NOT NULL DEFAULT true,
  CONSTRAINT uq_rank_users UNIQUE ("UserId", "RankId"));


-- categories (self-referencing)
CREATE TABLE IF NOT EXISTS "Categories" (
  "Id"         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Name"       text NOT NULL,
  "Slug"       text NOT NULL UNIQUE,
  "ParentId"  uuid REFERENCES "Categories"("Id") ON DELETE SET NULL);


-- spu (Standard Product Unit - product group)
CREATE TABLE IF NOT EXISTS "Spu" (
  "Id"           uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Name"         text NOT NULL,
  "SpuCode"     text UNIQUE,
  "CategoryId"  uuid REFERENCES "Categories"("Id") ON DELETE SET NULL,
  "Description"  text,
  "IsActive"    boolean NOT NULL DEFAULT true,
  "CreatedAt"   timestamptz NOT NULL DEFAULT now(),
  "UpdatedAt"   timestamptz NOT NULL DEFAULT now());


-- options (product group options)
CREATE TABLE IF NOT EXISTS "Options" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "SpuId"      uuid NOT NULL REFERENCES "Spu"("Id") ON DELETE CASCADE,
  "Name"        text NOT NULL,
  "SortOrder"  int NOT NULL DEFAULT 0);


-- option_values
CREATE TABLE IF NOT EXISTS "OptionValues" (
  "Id"           uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "OptionId"    uuid NOT NULL REFERENCES "Options"("Id") ON DELETE CASCADE,
  "Name"         text NOT NULL,
  "SortOrder"   int NOT NULL DEFAULT 0);

-- Units (Đơn vị tính)
CREATE TABLE IF NOT EXISTS "Units" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Name"        text NOT NULL,         -- Tên hiển thị: Kilogram, Hộp, Thùng, Cái,...
  "Code"        text NOT NULL UNIQUE,  -- Mã ngắn: kg, box, pcs, l,...
  "Description" text);


-- sku (product variant)
CREATE TABLE IF NOT EXISTS "Sku" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "SpuId"      uuid NOT NULL REFERENCES "Spu"("Id") ON DELETE CASCADE,
  "UnitId"     uuid NOT NULL REFERENCES "Units"("Id") ON DELETE RESTRICT,
  "SkuCode"    text NOT NULL UNIQUE,
  "Name"        text NOT NULL,
  "Price"       numeric(18,2) NOT NULL,
  "Image"       text NOT NULL,
  "IsActive"   boolean NOT NULL DEFAULT true,
  "BaseQtyPerSku" int NOT NULL DEFAULT 1);


-- sku_option_values (variant composition)
CREATE TABLE IF NOT EXISTS "SkuOptionValues" (
  "Id"         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "ValueId"   uuid NOT NULL REFERENCES "OptionValues"("Id") ON DELETE CASCADE,
  "SkuId"     uuid NOT NULL REFERENCES "Sku"("Id") ON DELETE CASCADE,
  CONSTRAINT uq_sku_option_value UNIQUE ("ValueId", "SkuId"));


-- spu_images
CREATE TABLE IF NOT EXISTS "SpuImages" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "SpuId"      uuid NOT NULL REFERENCES "Spu"("Id") ON DELETE CASCADE,
  "Url"         text NOT NULL,
  "Alt"         text,
  "SortOrder"  int NOT NULL DEFAULT 0);


-- batches (per SKU)
CREATE TABLE IF NOT EXISTS "Batches" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "SkuId"      uuid NOT NULL REFERENCES "Sku"("Id") ON DELETE CASCADE,
  "BatchCode"  text NOT NULL,
  "MfgDate"    date,
  "ExpDate"    date,
  "OnHand"		  int NOT NULL DEFAULT 0,
  "SellPrice"  numeric(18,2),
  CONSTRAINT uq_batches_sku_batch UNIQUE ("SkuId", "BatchCode"));


-- inventory_movements
CREATE TABLE IF NOT EXISTS "InventoryMovements" (
  "Id"                 uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "BatchId"           uuid NOT NULL REFERENCES "Batches"("Id") ON DELETE RESTRICT,
  "UnitId"            uuid NOT NULL REFERENCES "Units"("Id") ON DELETE RESTRICT,
  "RelatedOrderId"   uuid,
  "RelatedPurchaseId" uuid,
  "Type"               "InventoryType" NOT NULL,
  "Quantity"           numeric(18,3) NOT NULL,
  "MovedAt"           timestamptz NOT NULL DEFAULT now(),
  "Note"               text);


-- suppliers
CREATE TABLE IF NOT EXISTS "Suppliers" (
  "Id"             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "SupplierCode"  text UNIQUE,
  "Name"           text NOT NULL,
  "Phone"          text,
  "Email"          text,
  "Line"           text,
  "Ward"           text,
  "Province"       text,
  "Country"        text,
  "IsActive"      boolean NOT NULL DEFAULT true,
  "Notes"          text);


-- purchases
CREATE TABLE IF NOT EXISTS "Purchases" (
  "Id"             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "PoNumber"      text UNIQUE,
  "SupplierId"    uuid REFERENCES "Suppliers"("Id") ON DELETE SET NULL,
  "PurchasedAt"   timestamptz NOT NULL DEFAULT now(),
  "RefCode"       text,
  "Note"           text);


-- purchase_items
CREATE TABLE IF NOT EXISTS "PurchaseItems" (
  "Id"             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "PurchaseId"    uuid NOT NULL REFERENCES "Purchases"("Id") ON DELETE CASCADE,
  "BatchId"       uuid NOT NULL REFERENCES "Batches"("Id") ON DELETE RESTRICT,
  "UnitId"        uuid NOT NULL REFERENCES "Units"("Id") ON DELETE RESTRICT,
  "Quantity"       int NOT NULL CHECK ("Quantity" > 0),
  "ImportPrice"   numeric(18,2),
  "UnitConversionFactor" numeric(18,3) DEFAULT 1);


-- delivery_info (user addresses)
CREATE TABLE IF NOT EXISTS "DeliveryInfo" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "UserId"     uuid NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
  "FullName"   text NOT NULL,
  "Phone"       text,
  "Line"        text,
  "Ward"        text,
  "Province"    text,
  "Country"     text,
  "Status"      text,
  "IsDefault"  boolean NOT NULL DEFAULT false);

-- discounts (order-level rules)
CREATE TABLE IF NOT EXISTS "Discounts" (
  "Id"               uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Name"             text NOT NULL,
  "StartsAt"        timestamptz,
  "EndsAt"          timestamptz,
  "CustomerType"    text,
  "CustomerTier"    text,
  "Mode"             "DiscountMode" NOT NULL,
  "Value"            numeric(18,2) NOT NULL,
  "MaxDiscount"     numeric(18,2),
  "Active"           boolean NOT NULL DEFAULT true);

-- orders
CREATE TABLE IF NOT EXISTS "Orders" (
  "Id"               uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Code"             text NOT NULL UNIQUE,
  "UserId"          uuid NOT NULL REFERENCES "Users"("Id") ON DELETE RESTRICT,
  "DeliveryInfoId"     uuid NOT NULL REFERENCES "DeliveryInfo"("Id") ON DELETE RESTRICT,
  "Status"           "OrderStatus" NOT NULL DEFAULT 'PENDING',
  "PaymentMethod"   "PaymentMethod" NOT NULL,
  "ProductDiscount" numeric(18,2) NOT NULL DEFAULT 0,
  "VoucherDiscount" numeric(18,2) NOT NULL DEFAULT 0,
  "ShippingFee"     numeric(18,2) NOT NULL DEFAULT 0,
  "CreatedAt"       timestamptz NOT NULL DEFAULT now(),
  "UpdatedAt"       timestamptz NOT NULL DEFAULT now(),
  "DeliveredAt"     timestamptz);


-- order_items
CREATE TABLE IF NOT EXISTS "OrderItems" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "OrderId"    uuid NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
  "SkuId"      uuid NOT NULL REFERENCES "Sku"("Id") ON DELETE RESTRICT,
  "UnitId"     uuid NOT NULL REFERENCES "Units"("Id") ON DELETE RESTRICT,
  "DiscountId" uuid NOT NULL REFERENCES "Discounts"("Id") ON DELETE RESTRICT,
  "UnitPrice"  numeric(18,2) NOT NULL,
  "Quantity"    int NOT NULL CHECK ("Quantity" > 0),
  "LineTotal"  numeric(18,2) NOT NULL);


-- vouchers
CREATE TABLE IF NOT EXISTS "Vouchers" (
  "Id"                 uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "Code"               text NOT NULL UNIQUE,
  "StartsAt"          timestamptz,
  "EndsAt"            timestamptz,
  "Mode"               "DiscountMode" NOT NULL,
  "Value"              numeric(18,2) NOT NULL,
  "MaxDiscount"       numeric(18,2),
  "MinOrderTotal"    numeric(18,2) DEFAULT 0,
  "TotalLimit"        int,
  "PerCustomerLimit" int,
  "IsActive"          boolean NOT NULL DEFAULT true);


-- voucher_redemptions
CREATE TABLE IF NOT EXISTS "VoucherRedemptions" (
  "Id"           uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "VoucherId"   uuid NOT NULL REFERENCES "Vouchers"("Id") ON DELETE CASCADE,
  "UserId"      uuid NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
  "OrderId"     uuid NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
  "RedeemedAt"  timestamptz NOT NULL DEFAULT now(),
  CONSTRAINT uq_voucher_order UNIQUE ("VoucherId", "OrderId"));


-- payments
CREATE TABLE IF NOT EXISTS "Payments" (
  "Id"              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "OrderId"        uuid NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
  "Method"          "PaymentMethod" NOT NULL,
  "Status"          "PaymentStatus" NOT NULL DEFAULT 'PENDING',
  "Amount"          numeric(18,2) NOT NULL,
  "PaidAt"         timestamptz,

  -- Nhận diện giao dịch & nhà cung cấp
  "Provider"          text,                                     -- 'MOMO' | 'ZALOPAY' | 'VNPAY' | ...
  "TransactionRef"   text,                                     -- mã giao dịch nội bộ (merchant side)
  "ProviderTxnId"   text,                                     -- mã giao dịch phía cổng thanh toán (nếu khác transaction_ref)
  "RefundRef"        text,                                     -- mã giao dịch hoàn tiền (nếu có)

  -- Dữ liệu đối soát & truy vết
  "OrderCodeSnapshot" text,                                   -- bản chụp mã đơn tại thời điểm tạo payment
  "RawPayload"       jsonb,                                    -- payload phản hồi/notify từ cổng thanh toán (đã chuẩn hoá JSON)

  -- Siêu dữ liệu thêm (tùy chọn)
  "CreatedAt"        timestamptz NOT NULL DEFAULT now(),
  "UpdatedAt"        timestamptz NOT NULL DEFAULT now());


-- reviews
CREATE TABLE IF NOT EXISTS "Reviews" (
  "Id"          		uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "UserId"     		uuid NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
  "SkuId"      		uuid NOT NULL REFERENCES "Sku"("Id") ON DELETE CASCADE,
  "OrderItemId"   uuid NOT NULL REFERENCES "OrderItems"("Id") ON DELETE CASCADE,
  "Rating"      		int NOT NULL CHECK ("Rating" BETWEEN 1 AND 5),
  "Content"     		text,
  "Image"       		text,
  "CreatedAt"  		timestamptz NOT NULL DEFAULT now(),
  CONSTRAINT uq_review_purchase UNIQUE ("UserId", "SkuId", "OrderItemId"));


-- sku_discounts (map discount to SKU)
CREATE TABLE IF NOT EXISTS "SkuDiscounts" (
  "Id"            uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "DiscountId"   uuid NOT NULL REFERENCES "Discounts"("Id") ON DELETE CASCADE,
  "SkuId"        uuid NOT NULL REFERENCES "Sku"("Id") ON DELETE CASCADE,
  CONSTRAINT uq_discount_sku UNIQUE ("DiscountId", "SkuId"));


-- carts
CREATE TABLE IF NOT EXISTS "Carts" (
  "Id"               uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "UserId"          uuid NOT NULL UNIQUE REFERENCES "Users"("Id") ON DELETE CASCADE,
  "Status"           text NOT NULL);


-- cart_items
CREATE TABLE IF NOT EXISTS "CartItems" (
  "Id"          uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  "CartId"     uuid NOT NULL REFERENCES "Carts"("Id") ON DELETE CASCADE,
  "SkuId"      uuid NOT NULL REFERENCES "Sku"("Id") ON DELETE RESTRICT,
  "Quantity"    int NOT NULL CHECK ("Quantity" > 0),
  "UnitPrice"  numeric(18,2) NOT NULL,
  "Subtotal"    numeric(18,2) NOT NULL,
  CONSTRAINT uq_cart_item UNIQUE ("CartId", "SkuId"));


COMMIT;


-- ===================================================================
-- End of schema
-- ===================================================================



