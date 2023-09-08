export type Position = {
    name: string;
}

export type Employee = {
  id?: number;
  firstName: string;
  lastName: string;
  fullName?: string;
  address?: string;
  birthDate: string;
  startDate: string;
  position: Position;
  wage: number;
  isArchived?: boolean;
  endDate?: string;
}

export type Contract = {
  id: number;
  employee: Employee;
  position: Position;
  startDate: string;
}
