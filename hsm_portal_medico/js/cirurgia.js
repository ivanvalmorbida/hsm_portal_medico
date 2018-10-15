var vm = new Vue({
    el: '#app',
    data: function data() {
        return {
            rules: {
                required: (value) => !!value || 'Este campo é requerido!'
            }, valid: true,
            guia: null, autorizacao: null, data_autoriza: null, paci: null,
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
            if (this.$refs.form.validate()) {            
                this.$http.post("agenda.asmx/getAgendas", { anestesia: this.anestesia, tempo: this.tempo }).then((res) => {
                    this.agendas = JSON.parse(res.data.d)
                    $('#btn-buscar').removeAttr("disabled")
                })
            }
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
            var procedimentos = []
            var objAgendar = new Object()
            
            this.procedimentos.forEach(p => {
                procedimentos.push(p.procedimento)
            })

            objAgendar["sala"] = item.Sala
            objAgendar["paciente"] = this.paci
            objAgendar["tempo"] = this.tempo
            objAgendar["horaini"] = item.HoraIni
            objAgendar["guia"] = this.guia
			objAgendar["autorizacao"] = this.autorizacao
            objAgendar["data_autoriza"] = this.data_autoriza
			objAgendar["valid_autoriza"] = this.valid_autoriza
            objAgendar["procedimentos"] = procedimentos

            this.$http.post("agenda.asmx/Agendar", {obj: objAgendar}).then((res) => {
                location.href = 'Default.aspx'
            })
        },

        formatDate(date) {
            if (!date) return null

            const [year, month, day] = date.split('-')
            return `${day}/${month}/${year}`
        }
    },

    created() {
        var regex = /[?&]([^=#]+)=([^&#]*)/g, url = window.location.href, params = {}, match;
        while(match = regex.exec(url)) {
          params[match[1]] = match[2]
        }
        this.paci = params.p    
    }
});

/*parseDate(date) {
    if (!date) return null
    const [month, day, year] = date.split('/')
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
}*/
