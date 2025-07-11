// src/app/models/task.model.ts
import { TaskPriority } from '../enums/task-priority.enum';
import { TaskTag } from './tasktag.model';
//import { required, minLength, prop } from '../../validators/validation.decorators';
import { alphaNumeric,required,minLength,prop } from '@rxweb/reactive-form-validators';

export class Task {
  TaskId!: number;

  @alphaNumeric()
  @required()
  @minLength({ value: 3, message: 'Task title must be at least 3 characters long' })
  TaskTitle!: string;

  Description!: string;

  @required()
  @minLength({ value: 3, message: 'Priority must be at least 3 characters long' })
  Priority!: TaskPriority;

  @required()
  StartDay!: string;

  @required()
  EndDay!: string;

  @prop()
  @required()  // nếu bạn muốn ép bắt buộc chọn tag
  TagId!: number;

  IsDone!: boolean;
  TagName!: string;
  Color!: string;
  TaskTags!: TaskTag[];
  
}
