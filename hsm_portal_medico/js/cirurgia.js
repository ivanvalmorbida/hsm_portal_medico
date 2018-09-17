﻿var vm = new Vue({
    el: '#app',
    data: function data() {
        return {
            guia: null, autorizacao: null, data_autoriza: null,
            data_autoriza_Formatted: null, menu_data_autoriza: false,
            valid_autoriza: null, valid_autoriza_Formatted: null,
            menu_valid_autoriza: false, tempo: null, anestesia: null, procedimento: [],

            headers: [
                { text: 'Procedimento', align: 'left', value: 'procedimento' },
                { text: 'Descrição', align: 'left', value: 'descricao' },
                { text: 'Tempo', align: 'left', value: 'tempo' },
                { text: 'Excluir', value: 'procedimento', sortable: false }
            ],
            procedimentos: [],

            headers_agenda: [
                { text: 'Data hora inicio', align: 'left', value: 'HoraIniF', sortable: false },
                { text: 'Selecionar', value: 'item', sortable: false }
            ],
            agendas: [],

            loading: false,
            items: [],
            search: null,
        };
    },

    watch: {
        search(val) {
            val && this.querySelections(val)
        },

        data_autoriza(val) {
            this.data_autoriza_Formatted = this.formatDate(this.data_autoriza)
        },

        valid_autoriza(val) {
            this.valid_autoriza_Formatted = this.formatDate(this.valid_autoriza)
        },
    },

    methods: {
        getAgendas() {
            this.$http.post("medico.asmx/getMedico").then((res) => {
                //this.$http.post("agenda.asmx/getAgendas", { medico: 1, anestesia: 0, tempo: 0 }).then((res) => {
                this.$http.post("agenda.asmx/getAgendas", { medico: res.data.d, anestesia: this.anestesia, tempo: this.tempo }).then((res) => {
                    //console.dir(res.data.d)
                    this.agendas = JSON.parse(res.data.d)
                })
            })
        },

        querySelections(v) {
            if (v.length > 2) {
                this.loading = true
                this.$http.post("cirurgia.asmx/getCirurgiaNomCod", { strX: v })
                    .then((res) => {
                        var r = JSON.parse(res.data.d)
                        this.loading = false
                        this.items = r
                    })
            }
        },

        totalItem() {
            this.tempo = 0
            this.procedimentos.forEach(p => {
                console.dir(p.tempo)
                this.tempo += p.tempo
            });
        },

        delItem(item) {
            const index = this.procedimentos.indexOf(item)
            confirm('Confirma a exclusão deste item?') && this.procedimentos.splice(index, 1)
            this.totalItem()
        },

        addItem() {
            this.procedimentos.push({
                'procedimento': this.procedimento.value, 'descricao': this.procedimento.nome, 'tempo': this.procedimento.tempo
            })
            this.procedimento = null
            this.totalItem()
        },

        agendar(item) {
            console.dir(item.Sala)
            /*const index = this.procedimentos.indexOf(item)
            confirm('Confirma a exclusão deste item?') && this.procedimentos.splice(index, 1)
            this.totalItem()*/
        },

        formatDate(date) {
            if (!date) return null

            const [year, month, day] = date.split('-')
            return `${day}/${month}/${year}`
        }
    }
});

/*parseDate(date) {
    if (!date) return null
    const [month, day, year] = date.split('/')
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
}*/
