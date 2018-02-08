import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import Filter from './Filter'
import * as moment from 'moment';

interface FetchDataExampleState {
    proposals: ProposalData[];
    loading: boolean;
    initial: ProposalData[];
}

export class Application extends React.Component<{}, FetchDataExampleState> {
    constructor() {
        super();
        this.state = { proposals: [], loading: true, initial: [] };
        this.loadRemoteData(this.getRootUrl());
    }

    getRootUrl() {
        return 'http://localhost:53267/api/proposals/';
    }

    loadRemoteData(url : string) {
        fetch(url)
            .then(response => response.json() as Promise<ProposalData[]>)
            .then(data => {
                this.setState({ proposals: data, loading: false, initial: data });
            });
    }

    clientFilter(query: any) {
        const filered = this.state.initial.filter(query);
        this.setState({ proposals: filered, loading: false, initial : this.state.initial });
    }

    remoteFilter(query: any) {
        const queryString = new URLSearchParams();
        for (let key in query) {
            if (query.hasOwnProperty(key)) {
                queryString.append(key, query[key]);
            }
        }
        let url = this.getRootUrl() + "filter?" + queryString.toString();
        console.log(url);
        this.loadRemoteData(url);
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Application.renderTable(this.state.proposals);

        return <div>
            <h1>Voting example</h1>
            <p>Lets see existing proposals with votes count</p>
            <Filter clientFilter={this.clientFilter.bind(this)} remoteFilter={this.remoteFilter.bind(this)} />
            { contents }
        </div>;
    }

    private static renderTable(proposals: ProposalData[]) {
        return <div className='container-fluid'>
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Summary</th>
                        <th>Author</th>
                        <th>Votes Count</th>
                    </tr>
                </thead>
                <tbody>
                    {proposals.map(p =>
                        <tr key={p.id}>
                        <td>{moment(p.dateTime).format("DD MMM YYYY") }</td>
                        <td>{ p.summary }</td>
                        <td>{ p.author }</td>
                        <td>{ p.votesCount }</td>
                    </tr>
                )}
                </tbody>
            </table>
        </div>;
    }
}

export default Application;

interface ProposalData {
    id : number;
    dateTime: Date;
    summary: string;
    author: string;
    votesCount: number;
}
