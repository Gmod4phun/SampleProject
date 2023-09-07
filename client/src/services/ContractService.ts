import axios from 'axios'
const resourcePath = "Contract"

export default class ContractService {
    async getContractsForEmployee(empid: number) {
        return axios.get(`${resourcePath}/foremployee/${empid}`)
    }
}
