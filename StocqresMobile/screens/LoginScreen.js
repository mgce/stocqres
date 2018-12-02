import React, {Component} from 'react';
import { StyleSheet } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text } from 'native-base';
import {connect} from 'react-redux';

export default class LoginScreen extends Component{
    constructor(props){
        super(props);
        this.state = {
            login:'',
            password:''
        }
    }
    render(){
        return(
            <Container>
                {/* <Header/> */}
                <Content contentContainerStyle={styles.container}>
                    <Form style={styles.form}>
                        <Item floatingLabel>
                            <Label>Username</Label>
                            <Input/>
                        </Item>
                        <Item floatingLabel>
                            <Label>Password</Label>
                            <Input/>
                        </Item>
                        <Button primary style={styles.submit}>
                            <Text>Sign In</Text>
                        </Button>
                    </Form>
                </Content>
            </Container>
        )
    }
}

const styles = StyleSheet.create({
    container:{
      flex:1,
      paddingLeft: 15,
      paddingRight: 15,
      flexDirection:'column',
      alignItems:'center',
      justifyContent:'center'
  
    },
    submit:{
      marginTop: 40,
      justifyContent: 'center',
      alignSelf: 'stretch'
    },
    form: {
      padding: 10,
      paddingBottom: 30,
      borderRadius: 10,
      borderWidth: 1,
      borderColor: '#d6d7da',
      backgroundColor: '#ffffff',
      alignSelf: 'stretch'
    }
  });

// function mapStateToProps(state){
//     return{
//         username: state.username,
//         password: state.password
//     }
// }

// export default connect(mapStateToProps)(LoginScreen);