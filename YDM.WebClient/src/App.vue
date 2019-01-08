<template>
  <v-app id="inspire" dark>
    <v-card flat>
      <v-toolbar
        color="red darken-2"
        extended
        flat>
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
              <v-btn icon color="error"><v-icon>search</v-icon></v-btn>
            </v-toolbar>
            <v-divider></v-divider>
            <v-card-text style="min-height: 200px;">
              <v-layout ma-1 xs12 md7 align-center justify-center>
                  <v-card class="white--text">
                    <v-layout>
                      <v-flex>
                        <v-img
                          src="https://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU"
                          height="125px" contain
                        ></v-img>
                      </v-flex>
                      <v-flex xs7>
                        <v-card-title primary-title v-contain>
                          <div>
                            <div class="headline">Supermodel</div>
                            <div>Foster the People</div>
                            <a href="https://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU">tsMVj1VxSOU</a>
                          </div>
                        </v-card-title>
                      </v-flex>
                    </v-layout>
                    <v-divider light></v-divider>
                    <v-card-actions>
                      <span class="body-2 ma-2">hd720</span> <a href="https://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU">https://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU%herokuapp</a>
                    </v-card-actions>
                    <v-card-actions>
                      <span class="body-2 ma-2">hd720</span> <a href="https://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU">https://ydm.herokuapp.com/getimage.php?i=tsMVj1VxSOU%herokuapp</a>
                    </v-card-actions>
                  </v-card>
              </v-layout>
            </v-card-text>
            <!-- link to App and Github Pages-->
          </v-card>
        </v-flex>
      </v-layout>
    </v-card>
  <footer></footer>
  </v-app>
</template>
<script>
import Footer from './components/Footer.vue'
  export default {
    data: () => ({
      drawer: null
    }),
    components: {
      'footer': Footer,
    },
    props: {
      source: String
    }
  }
</script>