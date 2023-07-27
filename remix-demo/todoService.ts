export interface TodoModel {
  id: string | number;
  title: string;
  isComplete: boolean;
}

export default class TodoService {
  private static todos: TodoModel[] = [
    {
      id: "1",
      title: "Do Dishes",
      isComplete: false,
    },
  ];
  static getAll(): TodoModel[] {
    // Copies the array rather then returning the reference
    return this.todos.slice();
  }
  static getById(id: string) {
    return this.todos.find((t) => t.id === id) ?? null;
  }
  static add(todo: TodoModel) {
    this.todos.push(todo);
  }
  static upsert(id: string, todo: TodoModel) {
    let todoMutate = this.getById(id);
    if (!todoMutate) return;

    // This updates the obj ref in the array
    todoMutate = todo;
  }
  static delete(id: string) {
    this.todos = this.todos.filter((t) => t.id !== id);
  }
}
