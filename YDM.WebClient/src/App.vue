<template>
  <v-app id="inspire" dark>
    <v-card flat>
      <v-toolbar color="red darken-3" extended flat>
        <v-card-text>
          <h1 class="text-xs-center display-1 font-weight-light">YDM</h1>
        </v-card-text>
      </v-toolbar>
      <v-layout row pb-2 style="background: #303030;">
        <v-flex xs10 offset-xs1 md6 offset-md3>
          <v-card class="card--flex-toolbar" style="margin-top: -64px;background: #212121;">
            <v-toolbar card prominent>
              <v-text-field color="white" label="Youtube Video URL / Search / PlayList" style="margin-top: 12px"></v-text-field>
              <v-btn round color="red" dark><v-icon>get_apps</v-icon>GET</v-btn>
              <v-btn icon color="red"><v-icon>search</v-icon></v-btn>
            </v-toolbar>
            <v-divider></v-divider>
            <v-card-text style="min-height: 200px;">
              <v-flex text-xs-center>Get or Search for some videos</v-flex>
            </v-card-text>
            <!-- link to App and Git  hub Pages-->
          </v-card>
        </v-flex>
      </v-layout>
    </v-card>
  <footer></footer>
  </v-app>
</template>
<script>
  export default {
    data: () => ({
      drawer: null
    }),
    components: {
    },
    props: {
      source: String
    }
  }
</script>