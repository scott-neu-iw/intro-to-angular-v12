export interface TodoItem {
  id: number;
  name: string;
  description: string;
  createDate: Date;
  dueDate?: Date;
  assignedTo: string;
  completedDate?: Date;
  completedBy: string;
  completed: boolean;
  isPastDue: boolean;
  isLate: boolean;
}
