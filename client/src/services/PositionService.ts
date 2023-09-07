import axios from 'axios'
const resourcePath = "Position"

export default class PositionService {
    async getPositions() {
        return axios.get(`${resourcePath}/all`)
    }

    async deletePosition(name: string) {
        return axios.delete(`${resourcePath}/${name}`)
    }

    async addPosition(name: string) {
        return axios.post(`${resourcePath}/add/${name}`)
    }
}
