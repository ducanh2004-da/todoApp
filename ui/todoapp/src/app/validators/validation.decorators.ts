// validation.decorators.ts
import 'reflect-metadata';

export type ValidatorDescriptor = { name: string; args?: any };

const META_KEY = 'form:validators';

/**
 * Ghi metadata một validator lên property
 */
function addValidator(target: any, propertyKey: string, descriptor: ValidatorDescriptor) {
  const existing: ValidatorDescriptor[] =
    Reflect.getMetadata(META_KEY, target, propertyKey) || [];
  Reflect.defineMetadata(
    META_KEY,
    [...existing, descriptor],
    target,
    propertyKey
  );
}

/** Required decorator */
export function required() {
  return (target: any, propertyKey: string) => {
    addValidator(target, propertyKey, { name: 'required' });
  };
}

/** MinLength decorator */
export function minLength(length: number) {
  return (target: any, propertyKey: string) => {
    addValidator(target, propertyKey, { name: 'minLength', args: length });
  };
}

/** Bạn có thể thêm nhiều decorator khác tương tự */
export function maxLength(length: number) {
  return (target: any, propertyKey: string) => {
    addValidator(target, propertyKey, { name: 'maxLength', args: length });
  };
}

/** Prop decorator (chỉ để đánh dấu, không sinh validator) */
export function prop(defaultValue?: any) {
  return (target: any, propertyKey: string) => {
    if (defaultValue !== undefined) {
      target[propertyKey] = defaultValue;
    }
  };
}

/**
 * Lấy toàn bộ metadata validators của một class
 */
export function getValidators(target: any): Record<string, ValidatorDescriptor[]> {
  const keys = Object.getOwnPropertyNames(target.prototype);
  const result: Record<string, ValidatorDescriptor[]> = {};
  for (const key of keys) {
    const meta: ValidatorDescriptor[] =
      Reflect.getMetadata(META_KEY, target.prototype, key) || [];
    if (meta.length) {
      result[key] = meta;
    }
  }
  return result;
}
