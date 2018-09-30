<%@ Page Language="C#" Inherits="hsm_portal_medico.Default" %>
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <title>Agendamentos - HSM Portal Medico</title>
    <link rel='stylesheet prefetch' href='https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Material+Icons'>
    <link rel='stylesheet prefetch' href='https://unpkg.com/vuetify@1.0.19/dist/vuetify.min.css'>
</head>

<body>
    <div id="app" style="padding: 0px 20px 0px 20px">
        <v-app id="inspire">
            <v-container fluid grid-list-md>
                <p></p>
                <h1 class="display-1">HSM Portal Medico - Agendamento de Cirurgias</h1>
                <p></p>
                <p>
                    Usuario: {{medico.nome}}
                </p>
                <v-layout row wrap>
                    <v-flex d-flex>
                        <v-card color="indigo" dark>
                            <v-btn color="success" onclick="location.href = 'paciente.htm'">Fazer novo agendamento</v-btn>
                        </v-card>
                    </v-flex>
                </v-layout>
                <p></p>
                <h1 class="display-1">Cirurgias Agendadas:</h1>
                <v-data-table :headers="headers" :items="agendas" hide-actions>
                    <template slot="items" slot-scope="age">
                        <td>{{ age.item.data_hora }}</td>
                        <td>{{ age.item.paciente }}</td>
                        <td>{{ age.item.procedimento1 }}</td>
                        <td>
                            <v-btn icon class="mx-0" @click="detalhes(age.item)">
                                <v-icon color="pink">detalhes</v-icon>
                            </v-btn>
                        </td>
                    </template>
                </v-data-table>
            </v-container>

            <v-dialog v-model="dialog" width="80%">
                <v-card>
                    <v-card-title class="headline grey lighten-2" primary-title>
                        Agendamento
                    </v-card-title>

                    <v-card-text>
                        <v-text-field v-model="inicio" label="Inicio" readonly></v-text-field>
                        <v-text-field v-model="paciente" label="Paciente" readonly></v-text-field>
                        <v-text-field v-model="procedimento1" label="Procedimento1" readonly></v-text-field>
                    </v-card-text>

                    <v-divider></v-divider>

                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="primary" flat @click="dialog = false">
                            Ok
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-app>
    </div>
    <script src='https://unpkg.com/babel-polyfill/dist/polyfill.min.js'></script>
    <script src='https://unpkg.com/vue/dist/vue.js'></script>
    <script src='https://unpkg.com/vuetify@1.0.19/dist/vuetify.min.js'></script>
    <script src='https://unpkg.com/vue-resource@1.5.1/dist/vue-resource.min.js'></script>
    <script src='js/index.js'></script>
</body>

</html>