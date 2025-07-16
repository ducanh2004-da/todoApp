import { TaskPriority } from '../enums/task-priority.enum';
import { TaskTag } from './tasktag.model';

export class Task {
  TaskId!: number;
  TaskTitle!: string;
  Description!: string;
  Priority!: TaskPriority;
  StartDay!: string;
  EndDay!: string;
  TagId!: number;
  IsDone!: boolean;
  TagName!: string;
  Color!: string;
  TaskTags!: TaskTag[];
}