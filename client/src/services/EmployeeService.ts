import axios from 'axios'
const resourcePath = "Employee"

export default class EmployeeService {
    async updateEmployee(data: object) {
        return axios.patch(`${resourcePath}/${data.id}`, data)
    }

    async addEmployee(data: object) {
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
