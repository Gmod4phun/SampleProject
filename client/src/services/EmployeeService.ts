import axios from 'axios'
import { Employee } from '../interfaces/CustomDataTypes'
const resourcePath = "Employee"

export default class EmployeeService {
    async updateEmployee(data: Employee) {
        return axios.patch(`${resourcePath}/${data.id}`, data)
    }

    async addEmployee(data: Employee) {
        return axios.post(`${resourcePath}/add`, data)
    }

    async getEmployees() {
        return axios.get(`${resourcePath}/all/`)
    }

    async getCurrentEmployees() {
        return axios.get(`${resourcePath}/current/`)
    }

    async getPreviousEmployees() {
        return axios.get(`${resourcePath}/previous/`)
    }

    async deleteEmployee(id: number) {
        return axios.delete(`${resourcePath}/${id}/`)
    }

    async archiveEmployee(id: number) {
        return axios.post(`${resourcePath}/archive/${id}/`)
    }
}
