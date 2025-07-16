import { prop, required, minLength, alphaNumeric } from '@rxweb/reactive-form-validators';
import { TaskPriority } from '../enums/task-priority.enum';

export class UpdateTaskDTO {
  TaskId!: number;

  @alphaNumeric()
  @required()
  @minLength({ value: 3, message: 'Task title must be at least 3 characters' })
  TaskTitle!: string;

  @prop()
  Description?: string;

  @required()
  Priority!: TaskPriority;

  @required()
  StartDay!: string;

  @required()
  EndDay!: string;

  @prop()
  @required()
  TagId!: number;

  @prop()
  IsDone: boolean = false;
}