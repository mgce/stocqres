import React, {Component} from 'react';
import {connect} from 'react-redux';
import { goToAuth, goToHome } from './navigation'
import { View, Text } from 'native-base';

class Initialize extends React.PureComponent{
    async componentDidMount(){
        try{
            if(this.props.jwt != null || this.props.jwt != '')
                goToHome();
            else
                goToAuth();
        }
        catch(err){
            console.log('error: ' + err);
            goToAuth();
        }
    }
    render(){
        return(
            <View>
                <Text>Loading</Text>
            </View>
        )
    }
}

const mapStateToProps = (state) => {
    return{
        jwt: state.jwt
    }
}

export default connect(mapStateToProps)(Initialize);