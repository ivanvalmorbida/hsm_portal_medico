var vm = new Vue({
    el: '#app',
    data: function data() {
   
        return {
            vercpf: function (cpf) {
                if (cpf == null)
                    return false;

                if (cpf.length != 11 ||
                    cpf == "00000000000" ||
                    cpf == "11111111111" ||
                    cpf == "22222222222" ||
                    cpf == "33333333333" ||
                    cpf == "44444444444" ||
                    cpf == "55555555555" ||
                    cpf == "66666666666" ||
                    cpf == "77777777777" ||
                    cpf == "88888888888" ||
                    cpf == "99999999999")
                    return 'CPF Inválido!';
        
                add = 0;
        
                for (i = 0; i < 9; i++)
                        add += parseInt(cpf.charAt(i)) * (10 - i);
                rev = 11 - (add % 11);
                if (rev == 10 || rev == 11)
                    rev = 0;
                if (rev != parseInt(cpf.charAt(9)))
                    return 'CPF Inválido!';
                add = 0;
                        for (i = 0; i < 10; i++)
                        add += parseInt(cpf.charAt(i)) * (11 - i);
                rev = 11 - (add % 11);
                if (rev == 10 || rev == 11)
                    rev = 0;
                if (rev != parseInt(cpf.charAt(10)))
                    return 'CPF Inválido!';
                return true;
            },

            rules: {
                required: (value) => !!value || 'Este campo é requerido!',
                email: (value) => /.+@.+/.test(value) || 'E-mail inválido.',
                cpf: (value) => this.vercpf(value)
            },
          
            cpf: null, nome: null, nascimento: null, menu_nascimento: false,
            nascimento_Formatted: null, sexo: null, estadocivil_items: [], rg: null,
            estadocivil: null, profissao: null, profissao_items: [], convenio: null,
            convenio_items: [], plano: null, carteirinha: null, validade_cart: null,
            titular: null, pai: null, mae: null, cep: null, cidade: null, uf: null,
            bairro_items: [], bairro: null, contato: null, celular: null, telefone: null,
            email: null, validade_cart: null, menu_validade_cart: false,
            validade_cart_Formatted: null, dialog: false, formHasErrors: false, valid: true,
        };
    },

    watch: {
        nascimento(val) {
            this.nascimento_Formatted = this.formatDate(this.nascimento)
        },

        validade_cart(val) {
            this.validade_cart_Formatted = this.formatDate(this.validade_cart)
        },
    },

    created() {
        this.$http.post("paciente.asmx/getEstadoCivil")
            .then((res) => {
                this.estadocivil_items = JSON.parse(res.data.d)
            })

        this.$http.post("paciente.asmx/getProfissoes")
            .then((res) => {
                this.profissao_items = JSON.parse(res.data.d)
            })

        this.$http.post("paciente.asmx/getConvenio")
            .then((res) => {
                this.convenio_items = JSON.parse(res.data.d)
            })

        this.$http.post("paciente.asmx/getBairro")
            .then((res) => {
                this.bairro_items = JSON.parse(res.data.d)
            })
    },

    methods: {
        parseDate (date) {
            if (!date) return null
            
            day = date.substring(0,2)
            month = date.substring(2,4)
            year = date.substring(4,8)
            return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
          },
          
        gravar() {
            if (this.$refs.form.validate()) {
                var objPaciente = new Object()
                
                objPaciente["codigo"] = this.codigo
                objPaciente["cpf"] = this.cpf
                objPaciente["nome"] = this.nome
                objPaciente["nascimento"] = this.nascimento
                objPaciente["sexo"] = this.sexo
                objPaciente["rg"] = this.rg
                objPaciente["estadocivil"] = this.estadocivil
                objPaciente["profissao"] = this.profissao
                objPaciente["pai"] = this.pai
                objPaciente["mae"] = this.mae
                objPaciente["convenio"] = this.convenio.value
                objPaciente["plano"] = this.plano
                objPaciente["carteirinha"] = this.carteirinha
                objPaciente["titular"] = this.titular
                objPaciente["cep"] = this.cep
                objPaciente["validade_cart"] = this.validade_cart
                objPaciente["bairro"] = this.bairro
                objPaciente["contato"] = this.contato
                objPaciente["celular"] = this.celular
                objPaciente["telefone"] = this.telefone
                objPaciente["email"] = this.email
                
                this.$http.post("paciente.asmx/setPacienteCPF",{obj: objPaciente})
                .then((res) => {
                    location.href = 'cirurgia.htm?p=' + res.data.d + '&c=' + this.convenio
                })
            }
        },
        BuscarCPF() {
            if (this.cpf != undefined) { cpf = this.cpf.replace('.', '').replace('-', '') } else { cpf = '' }
            
            this.codigo = null
            this.nome = null
            this.nascimento = null
            this.sexo = null
            this.rg = null
            this.estadocivil = null
            this.profissao = null
            this.pai = null
            this.mae = null
            this.convenio = null
            this.plano = null
            this.carteirinha = null
            this.titular = null
            this.validade_cart = null
            this.cep = null
            this.bairro = null
            this.contato = null
            this.celular = null
            this.telefone = null
            this.email = null

            if (cpf.length == 11) {

                this.$http.post("paciente.asmx/getPacienteCPF", { strCPF: this.cpf })
                    .then((res) => {
                        paci = JSON.parse(res.data.d)
                        if (paci.length != 0) {
                            var paci = paci[0]
                            this.codigo = paci.codigo
                            this.nome = paci.nome
                            if (paci.nascimento != null) this.nascimento = paci.nascimento.substring(0,10)
                            this.sexo = paci.sexo
                            this.rg = paci.rg
                            this.estadocivil = paci.estadocivil
                            this.profissao = paci.profissao
                            this.pai = paci.pai
                            this.mae = paci.mae
                            this.convenio = paci.convenio
                            this.plano = paci.plano
                            this.carteirinha = paci.carteirinha
                            this.titular = paci.titular
                            if (paci.validade_cart != null) this.validade_cart = paci.validade_cart.substring(0,10)
                            this.cep = paci.cep
                            this.bairro = paci.bairro
                            this.contato = paci.contato
                            this.celular = paci.celular
                            this.telefone = paci.telefone.substring(0, 12).replace(' ', '')
                            this.email = paci.email

                            this.BuscarCEP()
                        }
                    })
            }
        },

        BuscarCEP() {
            this.$http.post("paciente.asmx/getCEP", { strCep: this.cep })
                .then((res) => {
                    cep = JSON.parse(res.data.d)
                    if (cep.length > 0) {
                        this.cidade = cep[0].Cidade
                        this.uf = cep[0].UF
                    }
                })
        },

        formatDate(date) {
            if (!date) return null

            const [year, month, day] = date.split('-')
            return `${day}/${month}/${year}`
        }
    }
});