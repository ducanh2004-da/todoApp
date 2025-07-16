import {Component, Input, Output, EventEmitter, OnChanges, SimpleChanges} from '@angular/core';
import { FormBuilder, Validators, ValidatorFn, FormGroup } from '@angular/forms';
import { Tag } from '../models/tag.model';
import { TaskStatus } from '../enums/task-status.enum';
import { Task } from '../models/task.model';
import { CreateTaskDTO } from '../models/create-task.dto';
import { UpdateTaskDTO } from '../models/update-task.dto';
import { getValidators,ValidatorDescriptor } from '../../validators/validation.decorators';
import { RxFormBuilder, IFormGroup } from '@rxweb/reactive-form-validators';

@Component({
  selector: 'app-task-form',
  templateUrl: './taskForm.component.html'
})
export class TaskFormComponent implements OnChanges {
  @Input() modalTitle!: string;
  @Input() tags: Tag[] = [];
  @Input() initialData: CreateTaskDTO | null = null;

  @Output() submitForm = new EventEmitter<CreateTaskDTO>();

  form!: IFormGroup<CreateTaskDTO>;

  // constructor(private fb: FormBuilder) {
  //   this.form = this.fb.group({
  //     TaskTitle: [''],
  //     Priority: ['Medium'],
  //     StartDay: [''],
  //     EndDay: [''],
  //     TagId: [null],
  //     Description: ['']
  //   });
  // }
  constructor(private formBuilder: RxFormBuilder) {
    this.form = this.formBuilder.formGroup(CreateTaskDTO) as IFormGroup<CreateTaskDTO>;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.initialData && this.initialData) {
      this.form.patchValue(this.initialData);
    }
    if (changes.modalTitle && this.modalTitle === 'Add Task') {
      // reset form when switching to Add
      // this.form.reset({
      //   TaskTitle: '',
      //   Priority: 'Medium',
      //   StartDay: '',
      //   EndDay: '',
      //   TagId: this.tags.length ? this.tags[0].TagId : null,
      //   Description: ''
      // });
       this.form.reset(new Task());                   // reset về giá trị mặc định trong model
      if (this.tags.length) {
        this.form.controls.TagId.setValue(this.tags[0].TagId);
      }
    }
  }

  // private buildForm() {
  //   const validatorsMeta = getValidators(Task);
  //   const groupConfig: { [key: string]: any } = {};

  //   for (const [key, descs] of Object.entries(validatorsMeta)) {
  //     // map descriptor → Angular ValidatorFn
  //     const fns: ValidatorFn[] = descs.map((d: ValidatorDescriptor) => {
  //       switch (d.name) {
  //         case 'required':   return Validators.required;
  //         case 'minLength':  return Validators.minLength(d.args);
  //         default:           return () => null;
  //       }
  //     });
  //     const defaultVal = (new Task() as any)[key];
  //     groupConfig[key] = [defaultVal, fns];
  //   }

  //   this.form = this.fb.group({
  //     ...groupConfig,
  //     // nếu cần thêm những field không có decorator:
  //     IsDone:    [(new Task()).IsDone],
  //     // v.v.
  //   });
  // }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    else if (this.form.touched) {
      console.log('Form has been modified');
    }
    // console.log('TaskTitle in form:', this.form.get('TaskTitle')?.value);
  //   this.form.setValue({
  //     ...this.form.value,
  //     TaskTitle: 'Đã thêm mới',
  //     Description: 'Mô tả công việc',
  // });
    this.submitForm.emit(this.form.value);
  }
}
