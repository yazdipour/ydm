<template>
  <v-app id="inspire" dark>
    <v-card flat>
      <v-toolbar color="red darken-3" extended flat>
        <v-card-text>
          <h2 class="text-xs-center display-1 font-weight-light">YDM</h2>
        </v-card-text>
      </v-toolbar>
      <v-layout row pb-2 style="background: #303030;">
        <v-flex xs10 offset-xs1 md6 offset-md3>
          <v-card class="card--flex-toolbar" style="margin-top: -64px;background: #212121;">
            <v-toolbar card prominent>
              <v-text-field color="white" label="Youtube Video URL / Search / PlayList" style="margin-top: 12px" v-model="inputData"></v-text-field>
              <v-btn icon color="red" v-on:click="getTheVideo"><v-icon>save_alt</v-icon></v-btn>
              <v-btn icon color="red" v-on:click="searchIt"><v-icon>search</v-icon></v-btn>
            </v-toolbar>
            <v-divider></v-divider>
            <!-- END TOOLBAR -->
            <p v-show="!videos.length" class="text-xs-center ma-4" >[[ Get/Search for a Video or Playlist ]]</p>
            <VideoView v-for="(item, index) in videos" :key="index" :video="videos[0]" :links="links"/>
            <Footer/>
          </v-card>
        </v-flex>
      </v-layout>
    </v-card>
  </v-app>
</template>
<script>
  import Footer from './components/Footer'
  import VideoView from './components/VideoView'

  export default {
    data: ()=>({
      inputData:"",
      videos:[],
      links: [[]]
    }),
    components: {Footer,VideoView},
    props: {
      source: String
    },
    methods: {
      l (...args) { 
        this.console.log(...args) 
      },
      searchIt(){
        if(this.inputData.includes('list=')){this.getPlayList();}
        else {
          alert();
        }
      },
      getTheVideo(){
        this.$http.headers.common['Access-Control-Allow-Origin'] = '*'
        const baseurl="https://cors-anywhere.herokuapp.com/https://ydm.herokuapp.com";
        this.$http.get(baseurl+'/getvideo.php?i='+this.inputData)
          .then((response) => {
            response.status;
            response.statusText;
            // get 'Expires' header
            response.headers.get('Expires');
            this.videos = response.data[0];
            this.links = response.data[1];
        });
      },
      getPlayList(){
      }
    },
  }
</script>