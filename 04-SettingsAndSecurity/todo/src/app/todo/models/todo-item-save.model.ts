export interface TodoItemSave {
  id: number;
  name: string;
  description: string;
  dueDate?: Date;
  assignedTo: string;
}
